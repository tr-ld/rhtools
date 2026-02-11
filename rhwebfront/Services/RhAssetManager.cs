using rhdata;
using RHWebFront.Models;

namespace RHWebFront.Services
{
    public class RhAssetManager : IRhAssetManager
    {
        private readonly IRhApiClient _apiClient;
        private readonly ILogger<RhAssetManager> _logger;

        public RhAssetManager(IRhApiClient apiClient, ILogger<RhAssetManager> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        private string TradeCurrency { get; set; } = "USD";

        #region Account
        private RHAccount _acctCache;
        private readonly SemaphoreSlim _acctLock = new(1, 1);

        public async Task<RHAccount> GetAccount()
        {
            if (_acctCache is not null) return _acctCache;

            await _acctLock.WaitAsync();
            try
            {
                if (_acctCache is not null) return _acctCache;

                _acctCache = await _apiClient.GetAcct();
                return _acctCache;
            }
            finally { _acctLock.Release(); }
        }

        public void InvalidateAccountCache()
        {
            _acctCache = null;
            _logger.LogDebug("Account cache invalidated");
        }
        #endregion

        #region Holdings
        private const int HOLDINGS_CACHE_SECONDS = 60;

        private string _holdingsCacheKey;
        private DateTimeOffset _holdingsCacheExpires = DateTimeOffset.MinValue;
        private readonly SemaphoreSlim _holdingsLock = new(1, 1);
        private RHHolding[] _holdingsCache;

        public async Task<RHHolding[]> GetHoldings(string[] symbols = null)
        {
            var key = symbols.CoalesceToAll();
            if (IsHoldingsCacheValidForKey(key, out var cachedQuick)) return cachedQuick;

            await _holdingsLock.WaitAsync();
            try
            {
                if (IsHoldingsCacheValidForKey(key, out var cached)) return cached;

                var results = await _apiClient.GetHoldings(symbols);
                var sorted = results.OrderBy(h => h.AssetCode).ToArray();

                _holdingsCache = sorted;
                _holdingsCacheKey = key;
                _holdingsCacheExpires = DateTimeOffset.UtcNow.AddSeconds(HOLDINGS_CACHE_SECONDS);

                return sorted;
            }
            finally { _holdingsLock.Release(); }
        }

        private bool IsHoldingsCacheValidForKey(string key, out RHHolding[] results)
        {
            if (_holdingsCache is not null && _holdingsCacheExpires > DateTimeOffset.UtcNow && string.Equals(_holdingsCacheKey, key, StringComparison.Ordinal))
            {
                results = _holdingsCache;
                return true;
            }

            results = [];
            return false;
        }

        public void InvalidateHoldingsCache()
        {
            _holdingsCache = null;
            _holdingsCacheKey = null;
            _holdingsCacheExpires = DateTimeOffset.MinValue;
            _logger.LogDebug("Holdings cache invalidated");
        }
        #endregion

        #region Assets
        private List<RHAssetSnapshot> _assetSnapshots;
        private readonly SemaphoreSlim _assetsLock = new(1, 1);

        public async Task<IReadOnlyList<RHAssetSnapshot>> GetAssets()
        {
            if (_assetSnapshots is not null) return _assetSnapshots.AsReadOnly();

            await _assetsLock.WaitAsync();
            try
            {
                if (_assetSnapshots is not null) return _assetSnapshots.AsReadOnly();

                _assetSnapshots = await SeedAssetsFromHoldings();
                _logger.LogInformation("Asset snapshots populated with {Count} assets", _assetSnapshots.Count);

                return _assetSnapshots.AsReadOnly();
            }
            finally { _assetsLock.Release(); }
        }

        private Task<List<RHAssetSnapshot>> SeedAssetsFromHoldings() { return BuildAssetsFromSymbols(); }
        private async Task<List<RHAssetSnapshot>> BuildAssetsFromSymbols(string[] seed = null)
        {
            var holdings = await GetHoldings(seed);
            var symbols = holdings.Select(h => h.AssetCode).ToArray();

            // Get bid/ask for retrieved symbols
            var bidAsks = await GetBestBidAsk(new Dictionary<string, string[]> { ["symbol"] = symbols });

            // Index bid/asks by base symbol
            var bidAsksBySymbol = bidAsks.ToDictionary(b => b.BaseSymbol);

            // Build snapshots
            var snapshots = new List<RHAssetSnapshot>();
            foreach (var holding in holdings)
            {
                bidAsksBySymbol.TryGetValue(holding.AssetCode, out var bidAsk);
                
                var availability = DataAvailability.Holding;
                if (bidAsk is not null) availability |= DataAvailability.BidAsk;

                snapshots.Add(new RHAssetSnapshot { Symbol = holding.AssetCode, Holding = holding, BidAsk = bidAsk, Availability = availability });
            }

            return snapshots;
        }
        #endregion

        #region Estimated Price
        public async Task<RHEstimatedPrice[]> GetEstimatedPrice(IDictionary<string, string[]> symbols)
        {
            var transformedSymbols = AppendCurrencyToValues(symbols);
            return await _apiClient.GetEstimatedPrice(transformedSymbols);
        }
        #endregion

        #region BidAsk
        public async Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> symbols)
        {
            var transformedSymbols = AppendCurrencyToValues(symbols);
            return await _apiClient.GetBestBidAsk(transformedSymbols);
        }
        #endregion

        #region Helper Methods
        private IDictionary<string, string[]> AppendCurrencyToValues(IDictionary<string, string[]> symbols)
        {
            if (symbols == null) return null;

            var transformed = new Dictionary<string, string[]>();
            foreach (var kvp in symbols) 
            { transformed[kvp.Key] = kvp.Value?.Select(v => $"{v}-{TradeCurrency}").ToArray(); }

            return transformed;
        }
        #endregion

        public void InvalidateAllCaches()
        {
            InvalidateAccountCache();
            InvalidateHoldingsCache();
            _logger.LogInformation("All caches invalidated");
        }
    }

    internal static class AssetManagerExtensions
    {
        internal static string CoalesceToAll(this string[] symbols)
        {
            return (symbols == null || symbols.Length == 0) ? "__all" : string.Join(',', symbols.OrderBy(s => s, StringComparer.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Extracts the base symbol from a compound symbol (e.g., "BTC" from "BTC-USD")
        /// </summary>
        extension(RHBidAsk bidAsk)
        {
            internal string BaseSymbol
            {
                get
                {
                    if (bidAsk.Symbol is null) return null;
                    
                    var index = bidAsk.Symbol.IndexOf('-');
                    return index > 0 ? bidAsk.Symbol[..index] : bidAsk.Symbol;
                }
            }
        }
    }
}
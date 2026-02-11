using rhdata;
using RHWebFront.Models;

namespace RHWebFront.Services
{
    public class RhAssetManager(IRhApiClient apiClient, ILogger<RhAssetManager> logger) : IRhAssetManager
    {
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

                _acctCache = await apiClient.GetAcct();
                return _acctCache;
            }
            finally { _acctLock.Release(); }
        }

        public void InvalidateAccountCache()
        {
            _acctCache = null;
            logger.LogDebug("Account cache invalidated");
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

                var results = await apiClient.GetHoldings(symbols);
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
            logger.LogDebug("Holdings cache invalidated");
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
                logger.LogInformation("Asset snapshots populated with {Count} assets", _assetSnapshots.Count);

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
            return await apiClient.GetEstimatedPrice(transformedSymbols);
        }
        #endregion

        #region BidAsk
        public async Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> symbols)
        {
            var transformedSymbols = AppendCurrencyToValues(symbols);
            return await apiClient.GetBestBidAsk(transformedSymbols);
        }
        #endregion

        #region Orders
        private const int OPEN_ORDERS_CACHE_SECONDS = 60;
        private const int CLOSED_ORDERS_CACHE_SECONDS = 60;

        private RHOrder[] _openOrdersCache;
        private DateTimeOffset _openOrdersCacheExpires = DateTimeOffset.MinValue;
        private readonly SemaphoreSlim _openOrdersLock = new(1, 1);

        private RHOrder[] _closedOrdersCache;
        private DateTimeOffset _closedOrdersCacheExpires = DateTimeOffset.MinValue;
        private readonly SemaphoreSlim _closedOrdersLock = new(1, 1);

        public async Task<RHOrder[]> GetOpenOrders()
        {
            if (_openOrdersCache is not null && _openOrdersCacheExpires > DateTimeOffset.UtcNow) return _openOrdersCache;

            await _openOrdersLock.WaitAsync();
            try
            {
                if (_openOrdersCache is not null && _openOrdersCacheExpires > DateTimeOffset.UtcNow) return _openOrdersCache;

                var results = await apiClient.GetOpenOrders();
                
                _openOrdersCache = results;
                _openOrdersCacheExpires = DateTimeOffset.UtcNow.AddSeconds(OPEN_ORDERS_CACHE_SECONDS);

                return results;
            }
            finally { _openOrdersLock.Release(); }
        }

        public async Task<RHOrder[]> GetClosedOrders()
        {
            if (_closedOrdersCache is not null && _closedOrdersCacheExpires > DateTimeOffset.UtcNow) return _closedOrdersCache;

            await _closedOrdersLock.WaitAsync();
            try
            {
                if (_closedOrdersCache is not null && _closedOrdersCacheExpires > DateTimeOffset.UtcNow) return _closedOrdersCache;

                var results = await GetOrders(new Dictionary<string, string[]> { ["state"] = ["filled", "canceled", "failed"] });
                
                _closedOrdersCache = results;
                _closedOrdersCacheExpires = DateTimeOffset.UtcNow.AddSeconds(CLOSED_ORDERS_CACHE_SECONDS);

                return results;
            }
            finally { _closedOrdersLock.Release(); }
        }

        public async Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams = null)
        {
            var parameters = queryParams is null || queryParams.Count == 0 ? new Dictionary<string, string[]>() : queryParams;
            return await apiClient.GetOrders(parameters);
        }

        public void InvalidateOpenOrdersCache()
        {
            _openOrdersCache = null;
            _openOrdersCacheExpires = DateTimeOffset.MinValue;
            logger.LogDebug("Open orders cache invalidated");
        }

        public void InvalidateClosedOrdersCache()
        {
            _closedOrdersCache = null;
            _closedOrdersCacheExpires = DateTimeOffset.MinValue;
            logger.LogDebug("Closed orders cache invalidated");
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
            InvalidateOpenOrdersCache();
            InvalidateClosedOrdersCache();
            logger.LogInformation("All caches invalidated");
        }
    }

    internal static class AssetManagerExtensions
    {
        internal static string CoalesceToAll(this string[] symbols)
        { return (symbols == null || symbols.Length == 0) ? "__all" : string.Join(',', symbols.OrderBy(s => s, StringComparer.OrdinalIgnoreCase)); }

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
using Microsoft.Extensions.Caching.Memory;
using rhdata;
using RHWebFront.Models;

namespace RHWebFront.Services
{
    public class RhAssetManager(IRhApiClient apiClient, ILogger<RhAssetManager> logger, IMemoryCache cache) : IRhAssetManager
    {
        private readonly string _instanceId = Guid.NewGuid().ToString()[..8];
        private string TradeCurrency { get; set; } = "USD";

        #region Cache Keys
        private const string ACCOUNT_CACHE_KEY = "Account";
        private const string HOLDINGS_CACHE_KEY_PREFIX = "Holdings_";
        private const string ASSETS_CACHE_KEY = "Assets";
        private const string ORDERS_CACHE_KEY = "AllOrders";
        #endregion

        #region Account
        private readonly SemaphoreSlim _acctLock = new(1, 1);

        public async Task<RHAccount> GetAccount()
        {
            if (cache.TryGetValue(ACCOUNT_CACHE_KEY, out RHAccount cachedAccount)) return cachedAccount;

            await _acctLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(ACCOUNT_CACHE_KEY, out cachedAccount)) return cachedAccount;

                var account = await apiClient.GetAcct();
                cache.Set(ACCOUNT_CACHE_KEY, account, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
                
                return account;
            }
            finally { _acctLock.Release(); }
        }

        public void InvalidateAccountCache()
        {
            cache.Remove(ACCOUNT_CACHE_KEY);
            logger.LogDebug("[{InstanceId}] Account cache invalidated", _instanceId);
        }
        #endregion

        #region Holdings
        private const int HOLDINGS_CACHE_SECONDS = 60;
        private readonly SemaphoreSlim _holdingsLock = new(1, 1);

        public async Task<RHHolding[]> GetHoldings(string[] symbols = null)
        {
            var key = GetHoldingsCacheKey(symbols);
            if (cache.TryGetValue(key, out RHHolding[] cachedHoldings)) return cachedHoldings;

            await _holdingsLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(key, out cachedHoldings)) return cachedHoldings;

                var results = await apiClient.GetHoldings(symbols);
                var sorted = results.OrderBy(h => h.AssetCode).ToArray();

                cache.Set(key, sorted, TimeSpan.FromSeconds(HOLDINGS_CACHE_SECONDS));

                return sorted;
            }
            finally { _holdingsLock.Release(); }
        }

        private static string GetHoldingsCacheKey(string[] symbols)
        {
            var symbolKey = symbols.CoalesceToAll();
            return $"{HOLDINGS_CACHE_KEY_PREFIX}{symbolKey}";
        }

        public void InvalidateHoldingsCache()
        {
            cache.Remove(GetHoldingsCacheKey(null));
            logger.LogDebug("[{InstanceId}] Holdings cache invalidated", _instanceId);
        }
        #endregion

        #region Assets
        private readonly SemaphoreSlim _assetsLock = new(1, 1);

        public async Task<IReadOnlyList<RHAssetSnapshot>> GetAssets()
        {
            if (cache.TryGetValue(ASSETS_CACHE_KEY, out List<RHAssetSnapshot> cachedAssets)) return cachedAssets.AsReadOnly();

            await _assetsLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(ASSETS_CACHE_KEY, out cachedAssets)) return cachedAssets.AsReadOnly();

                var assets = await SeedAssetsFromHoldings();
                cache.Set(ASSETS_CACHE_KEY, assets, new MemoryCacheEntryOptions { Priority = CacheItemPriority.Normal });
                logger.LogDebug("[{InstanceId}] Asset snapshots populated with {Count} assets", _instanceId, assets.Count);

                return assets.AsReadOnly();
            }
            finally { _assetsLock.Release(); }
        }

        private Task<List<RHAssetSnapshot>> SeedAssetsFromHoldings() { return BuildAssetsFromSymbols(); }
        
        private async Task<List<RHAssetSnapshot>> BuildAssetsFromSymbols(string[] seed = null)
        {
            var holdings = await GetHoldings(seed);
            var symbols = holdings.Select(h => h.AssetCode).ToArray();

            var bidAsks = await GetBestBidAsk(new Dictionary<string, string[]> { ["symbol"] = symbols });
            var bidAsksBySymbol = bidAsks.ToDictionary(b => b.BaseSymbol);

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
        private const int ORDERS_CACHE_SECONDS = 60;
        private readonly SemaphoreSlim _ordersLock = new(1, 1);

        public async Task<RHOrder[]> GetOpenOrders()
        {
            var allOrders = await GetAllOrdersCached();
            return allOrders.Where(o => o.State is "open" or "partially_filled").ToArray();
        }

        public async Task<RHOrder[]> GetClosedOrders()
        {
            var allOrders = await GetAllOrdersCached();
            return allOrders.Where(o => o.State is "filled" or "canceled" or "failed").ToArray();
        }

        private async Task<RHOrder[]> GetAllOrdersCached()
        {
            if (cache.TryGetValue(ORDERS_CACHE_KEY, out RHOrder[] cachedOrders)) return cachedOrders;

            await _ordersLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(ORDERS_CACHE_KEY, out cachedOrders)) return cachedOrders;

                var results = await GetOrders(null);
                cache.Set(ORDERS_CACHE_KEY, results, TimeSpan.FromSeconds(ORDERS_CACHE_SECONDS));
                logger.LogDebug("[{InstanceId}] Cached {Count} orders in IMemoryCache for {Seconds}s", _instanceId, results.Length, ORDERS_CACHE_SECONDS);

                return results;
            }
            finally { _ordersLock.Release(); }
        }

        public async Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams = null)
        {
            var parameters = queryParams is null || queryParams.Count == 0 ? new Dictionary<string, string[]>() : queryParams;
            return await apiClient.GetOrders(parameters);
        }

        public void InvalidateOrdersCache()
        {
            cache.Remove(ORDERS_CACHE_KEY);
            logger.LogDebug("[{InstanceId}] Orders cache invalidated from IMemoryCache", _instanceId);
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
            InvalidateOrdersCache();
            cache.Remove(ASSETS_CACHE_KEY);
            logger.LogInformation("[{InstanceId}] All caches invalidated", _instanceId);
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
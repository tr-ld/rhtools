using abstractions.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using rhdata;
using rhdata.Models;
using RHWebFront.Config;
using RHWebFront.Constants;

namespace RHWebFront.Services
{
    public class RhAssetManager(IRhApiClient apiClient, ILogger<RhAssetManager> logger, IMemoryCache cache, IOptionsSnapshot<AppConfig> appConfig, IOptionsSnapshot<CacheConfig> cacheConfig) : IRhAssetManager
    {
        private readonly string _instanceId = Guid.NewGuid().ToString()[..8];
        private readonly AppConfig _appConfig = appConfig.Value;
        private readonly CacheConfig _cacheConfig = cacheConfig.Value;

        #region Account
        private readonly SemaphoreSlim _acctLock = new(1, 1);

        public async Task<RHAccount> GetAccount()
        {
            if (cache.TryGetValue(CacheKeys.Account, out RHAccount cachedAccount)) return cachedAccount;

            await _acctLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(CacheKeys.Account, out cachedAccount)) return cachedAccount;

                var account = await apiClient.GetAcct();
                cache.Set(CacheKeys.Account, account, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
                
                return account;
            }
            finally { _acctLock.Release(); }
        }
        #endregion

        #region Holdings
        private readonly SemaphoreSlim _holdingsLock = new(1, 1);

        public async Task<RHHolding[]> GetHoldings(string[] symbols = null)
        {
            var key = CacheKeys.Holdings(symbols);
            if (cache.TryGetValue(key, out RHHolding[] cachedHoldings)) return cachedHoldings;

            await _holdingsLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(key, out cachedHoldings)) return cachedHoldings;

                var results = await apiClient.GetHoldings(symbols);
                var sorted = results.OrderBy(h => h.AssetCode).ToArray();

                cache.Set(key, sorted, TimeSpan.FromSeconds(_cacheConfig.HoldingsCacheSeconds));

                return sorted;
            }
            finally { _holdingsLock.Release(); }
        }
        #endregion

        #region Assets
        public async Task<IReadOnlyList<RHAssetSnapshot>> GetAssets()
        {
            var assets = await BuildAssetsFromSymbols();
            return assets.AsReadOnly();
        }

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

        #region Trading Pairs
        private readonly SemaphoreSlim _tradingPairsLock = new(1, 1);

        public async Task<RHTradingPair[]> GetTradingPairs()
        {
            if (cache.TryGetValue(CacheKeys.TradingPairs, out RHTradingPair[] cachedPairs)) return cachedPairs;

            await _tradingPairsLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(CacheKeys.TradingPairs, out cachedPairs)) return cachedPairs;

                var pairs = await apiClient.GetTradingPairs();
                cache.Set(CacheKeys.TradingPairs, pairs, TimeSpan.FromHours(_cacheConfig.TradingPairsCacheHours));
                logger.LogDebug("[{InstanceId}] Cached {Count} trading pairs for {Hours}h", _instanceId, pairs.Length, _cacheConfig.TradingPairsCacheHours);

                return pairs;
            }
            finally { _tradingPairsLock.Release(); }
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
        private readonly SemaphoreSlim _bidAskLock = new(1, 1);

        public async Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> symbols)
        {
            var transformedSymbols = AppendCurrencyToValues(symbols);
            var symbolList = transformedSymbols?["symbol"];
            var cacheKey = CacheKeys.Holdings(symbolList);

            if (cache.TryGetValue(cacheKey, out RHBidAsk[] cachedBidAsks)) return cachedBidAsks;

            await _bidAskLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(cacheKey, out cachedBidAsks)) return cachedBidAsks;

                var results = await apiClient.GetBestBidAsk(transformedSymbols);
                cache.Set(cacheKey, results, TimeSpan.FromSeconds(_cacheConfig.BidAskCacheSeconds));

                return results;
            }
            finally { _bidAskLock.Release(); }
        }
        #endregion

        #region Orders
        private readonly SemaphoreSlim _ordersLock = new(1, 1);

        public async Task<RHOrder[]> GetOpenOrders()
        {
            var allOrders = await GetAllOrders();
            return allOrders.Where(o => o.State is "open" or "partially_filled").ToArray();
        }

        public async Task<RHOrder[]> GetClosedOrders()
        {
            var allOrders = await GetAllOrders();
            return allOrders.Where(o => o.State is "filled" or "canceled" or "failed").ToArray();
        }

        public async Task<RHOrder[]> GetAllOrders()
        {
            if (cache.TryGetValue(CacheKeys.AllOrders, out RHOrder[] cachedOrders)) return cachedOrders;

            await _ordersLock.WaitAsync();
            try
            {
                if (cache.TryGetValue(CacheKeys.AllOrders, out cachedOrders)) return cachedOrders;

                var results = await GetOrders(null);
                cache.Set(CacheKeys.AllOrders, results, TimeSpan.FromSeconds(_cacheConfig.OrdersCacheSeconds));
                logger.LogDebug("[{InstanceId}] Cached {Count} orders in IMemoryCache for {Seconds}s", _instanceId, results.Length, _cacheConfig.OrdersCacheSeconds);

                return results;
            }
            finally { _ordersLock.Release(); }
        }

        private async Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams = null)
        {
            var parameters = queryParams is null || queryParams.Count == 0 ? new Dictionary<string, string[]>() : queryParams;
            return await apiClient.GetOrders(parameters);
        }
        #endregion

        #region Cache Invalidation
        public void InvalidateAccountCache()
        {
            cache.Remove(CacheKeys.Account);
            logger.LogDebug("[{InstanceId}] Account cache invalidated", _instanceId);
        }

        public void InvalidateHoldingsCache()
        {
            cache.Remove(CacheKeys.Holdings(null));
            logger.LogDebug("[{InstanceId}] Holdings cache invalidated", _instanceId);
        }

        public void InvalidateBidAskCache()
        {
            cache.Remove(CacheKeys.Holdings(null));
            logger.LogDebug("[{InstanceId}] BidAsk cache invalidated", _instanceId);
        }

        public void InvalidateOrdersCache()
        {
            cache.Remove(CacheKeys.AllOrders);
            logger.LogDebug("[{InstanceId}] Orders cache invalidated from IMemoryCache", _instanceId);
        }

        public void InvalidateAllCaches()
        {
            InvalidateAccountCache();
            InvalidateHoldingsCache();
            InvalidateBidAskCache();
            InvalidateOrdersCache();
            logger.LogInformation("[{InstanceId}] All caches invalidated", _instanceId);
        }
        #endregion

        #region Helper Methods
        private IDictionary<string, string[]> AppendCurrencyToValues(IDictionary<string, string[]> symbols)
        {
            if (symbols == null) return null;

            var transformed = new Dictionary<string, string[]>();
            foreach (var kvp in symbols) 
            { transformed[kvp.Key] = kvp.Value?.Select(v => $"{v}-{_appConfig.TradeCurrency}").ToArray(); }

            return transformed;
        }
        #endregion
    }

    internal static class AssetManagerExtensions
    {
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
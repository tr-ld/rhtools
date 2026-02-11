using rhdata;
using RHWebFront.Models;

namespace RHWebFront.Services
{
    public interface IRhAssetManager
    {
        Task<RHAccount> GetAccount();
        Task<RHHolding[]> GetHoldings(string[] symbols = null);
        Task<IReadOnlyList<RHAssetSnapshot>> GetAssets();
        Task<RHEstimatedPrice[]> GetEstimatedPrice(IDictionary<string, string[]> queryParams);
        Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> queryParams);
        Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams = null);
        Task<RHOrder[]> GetOpenOrders();
        Task<RHOrder[]> GetClosedOrders();
        
        void InvalidateAccountCache();
        void InvalidateHoldingsCache();
        void InvalidateAllCaches();
        void InvalidateOrdersCache();
    }
}
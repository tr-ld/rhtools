using rhdata;

namespace RHWebFront.Services
{
    public interface IRhApiClient
    {
        Task<RHAccount> GetAcct();
        Task<RHHolding[]> GetHoldings(string[] symbols);
        Task<RHTradingPair[]> GetTradingPairs();
        Task<RHEstimatedPrice[]> GetEstimatedPrice(IDictionary<string, string[]> queryParams);
        Task<RHBidAsk[]> GetBestBidAsk(IDictionary<string, string[]> queryParams);
        Task<RHOrder> GetOrder(Guid orderId);
        Task<RHOrder[]> GetOrders(IDictionary<string, string[]> queryParams);
        Task<RHOrder[]> GetOpenOrders();
    }
}
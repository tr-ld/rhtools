using Microsoft.Extensions.Options;
using rhapi.Poco.Market;
using rhapi.Poco.Order;
using rhapi.Poco.Responses;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Json;

namespace rhapi.Services
{
    public interface IRhCryptoMarket
    {
        public Task<RhAccountResponse> GetAccount();
        public Task<RhHoldingsResponse> GetHoldings(string[] symbols);

        public Task<RhOrder> GetOrder(Guid orderId);
        public Task<RhOrdersResponse> GetOrders(RhOrderParams orderParams);
        public Task<RhOrder> PlaceOrder(RhPlaceOrderParams orderParams, IOptions<JsonOptions> options);

        public Task<RhTradingPairsResponse> GetTradingPairs(RhTradingPairsParams pairParams);
        public Task<RhEstimatedPriceResponse> GetEstimatedPrice(RhEstimatedPriceParams estParams);
        public Task<RhBidAskResponse> GetBestBidAsk(RhBidAskParams bidAskParams);
    }
}

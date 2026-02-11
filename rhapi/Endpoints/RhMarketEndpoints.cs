using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using rhapi.Poco.Market;
using rhapi.Poco.Responses;
using rhapi.Services;

namespace rhapi.Endpoints
{
    public class RhMarketEndpoints(IRhCryptoMarket market) : RhEndpoints(market)
    {
        public const string MARKET_GROUP = @"/market";
        public const string GET_TRADING_PAIRS = @"/getTradingPairs";
        public const string GET_EST_PRICE = @"/getEstimatedPrice";
        public const string GET_BID_ASK = @"/getBestBidAsk";

        public Task<RhTradingPairsResponse> GetTradingPairsAsync(RhTradingPairsParams pairParams) { return _market.GetTradingPairs(pairParams); }
        public Task<RhEstimatedPriceResponse> GetEstimatedPriceAsync(RhEstimatedPriceParams estParams) { return _market.GetEstimatedPrice(estParams); }
        public Task<RhBidAskResponse> GetBestBidAskAsync(RhBidAskParams bidAskParams) { return _market.GetBestBidAsk(bidAskParams); }
    }

    internal static class RhMarketExtensions
    {
        internal static RouteGroupBuilder MapMarketEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(RhMarketEndpoints.MARKET_GROUP);

            group.MapGet(RhMarketEndpoints.GET_TRADING_PAIRS, (RhMarketEndpoints ep, RhTradingPairsParams queryParams) => ep.GetTradingPairsAsync(queryParams));
            group.MapGet(RhMarketEndpoints.GET_EST_PRICE, (RhMarketEndpoints ep, [AsParameters] RhEstimatedPriceParams queryParams) => ep.GetEstimatedPriceAsync(queryParams));
            group.MapGet(RhMarketEndpoints.GET_BID_ASK, (RhMarketEndpoints ep, [AsParameters] RhBidAskParams queryParams) => ep.GetBestBidAskAsync(queryParams));

            return group;
        }
    }
}

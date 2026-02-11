using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using rhapi.Services;
using System.Threading.Tasks;
using rhapi.Poco.Responses;

namespace rhapi.Endpoints
{
    public class RhAccountEndpoints(IRhCryptoMarket market) : RhEndpoints(market)
    {
        public const string ACCT_GROUP = @"/rhacct";
        public const string GET_ACCT = @"/getAcct";
        public const string GET_HOLDINGS = @"/getHoldings";

        public Task<RhAccountResponse> GetAccountAsync() { return _market.GetAccount(); }
        public Task<RhHoldingsResponse> GetHoldings(string[] symbol) { return _market.GetHoldings(symbol); }
    }

    internal static class RhAccountExtensions
    {
        internal static RouteGroupBuilder MapAccountEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(RhAccountEndpoints.ACCT_GROUP);

            group.MapGet(RhAccountEndpoints.GET_ACCT, (RhAccountEndpoints ep) => ep.GetAccountAsync());
            group.MapGet(RhAccountEndpoints.GET_HOLDINGS, (RhAccountEndpoints ep, [FromQuery] string[] symbols) => ep.GetHoldings(symbols));

            return group;
        }
    }
}

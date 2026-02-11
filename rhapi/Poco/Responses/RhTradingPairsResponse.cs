using rhapi.Poco.Market;

namespace rhapi.Poco.Responses
{
    public class RhTradingPairsResponse : RhPagingResponse
    {
        public TradingPair[] Results { get; set; } = [];
    }
}

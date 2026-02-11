using rhapi.Poco.Market;
using System;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhBidAskResponse
    {
        public BidAskPrice[] Results { get; set; } = [];
    }
}

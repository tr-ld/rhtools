using System;
using rhapi.Poco.Market;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhEstimatedPriceResponse : RhPagingResponse
    {
        public PriceEstimate[] Results { get; set; } = [];
    }
}

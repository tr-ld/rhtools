using System;
using Newtonsoft.Json;
using rhapi.Enums;

namespace rhapi.Poco.Market
{
    [Serializable]
    public class PriceEstimate : BidAskPrice
    {
        [JsonProperty("side")]
        public EstimateSide Side { get; set; }
    }
}

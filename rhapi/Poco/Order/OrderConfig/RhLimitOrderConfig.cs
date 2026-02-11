using System;
using Newtonsoft.Json;
using rhapi.Enums;

namespace rhapi.Poco.Order.OrderConfig
{
    [Serializable]
    public class RhLimitOrderConfig : RhMarketOrderConfig
    {
        [JsonProperty("quote_amount")]
        public decimal? QuoteAmount { get; set; }

        [JsonProperty("limit_price")]
        public decimal? LimitPrice { get; set; }

        [JsonProperty("time_in_force")]
        public TimeInForce? TimeInForce { get; set; }
    }
}

using rhapi.Enums;
using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Order.OrderConfig
{
    [Serializable]
    public class RhStopLossOrderConfig : RhMarketOrderConfig
    {
        [JsonProperty("quote_amount")]
        public decimal? QuoteAmount { get; set; }

        [JsonProperty("stop_price")]
        public decimal? StopPrice { get; set; }

        [JsonProperty("time_in_force")]
        public TimeInForce? TimeInForce { get; set; }
    }
}

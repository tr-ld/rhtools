using Newtonsoft.Json;
using rhapi.Enums;
using System;

namespace rhapi.Poco.Market
{
    [Serializable]
    public class TradingPair
    {
        [JsonProperty("asset_code")]
        public string AssetCode { get; set; }

        [JsonProperty("quote_code")]
        public string QuoteCode { get; set; }

        [JsonProperty("quote_increment")]
        public string QuoteIncrement { get; set; }

        [JsonProperty("asset_increment")]
        public string AssetIncrement { get; set; }

        [JsonProperty("max_order_size")]
        public string MaxOrderSize { get; set; }
        
        [JsonProperty("min_order_size")]
        public string MinOrderSize { get; set; }

        [JsonProperty("status")]
        public TradingStatus Status { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

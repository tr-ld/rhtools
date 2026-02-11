using Newtonsoft.Json;
using System;

namespace rhapi.Poco.Market
{
    [Serializable]
    public class BidAskPrice
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("bid_inclusive_of_sell_spread")]
        public decimal BidInclusiveOfSellSpread { get; set; }

        [JsonProperty("sell_spread")]
        public decimal SellSpread { get; set; }

        [JsonProperty("ask_inclusive_of_buy_spread")]
        public decimal AskInclusiveOfBuySpread { get; set; }

        [JsonProperty("buy_spread")]
        public decimal BuySpread { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
    }
}

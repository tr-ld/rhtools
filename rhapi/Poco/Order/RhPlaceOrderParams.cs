using Newtonsoft.Json;
using rhapi.Enums;
using rhapi.Poco.Order.OrderConfig;
using System;

namespace rhapi.Poco.Order
{
    [Serializable]
    public class RhPlaceOrderParams
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("client_order_id")]
        public Guid ClientOrderId { get; set; }

        [JsonProperty("side")]
        public Side Side { get; set; }

        [JsonProperty("type")]
        public OrderType Type { get; set; }

        [JsonProperty("market_order_config")]
        public RhMarketOrderConfig MarketOrderConfig { get; set; }

        [JsonProperty("limit_order_config")]
        public RhLimitOrderConfig LimitOrderConfig { get; set; }

        [JsonProperty("stop_loss_order_config")]
        public RhStopLossOrderConfig StopLossOrderConfig { get; set; }

        [JsonProperty("stop_limit_order_config")]
        public RhStopLimitOrderConfig StopLimitOrderConfig { get; set; }
    }
}

using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Order.OrderConfig
{
    [Serializable]
    public class RhStopLimitOrderConfig : RhLimitOrderConfig
    {
        [JsonProperty("stop_price")]
        public decimal? StopPrice { get; set; }
    }
}

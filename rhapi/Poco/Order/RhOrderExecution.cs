using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Order
{
    [Serializable]
    public class RhOrderExecution
    {
        [JsonProperty("effective_price")]
        public decimal EffectivePrice { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}

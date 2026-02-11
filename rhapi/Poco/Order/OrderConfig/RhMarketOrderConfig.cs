using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Order.OrderConfig
{
    [Serializable]
    public class RhMarketOrderConfig
    {
        [JsonProperty("asset_quantity")]
        public decimal? AssetQuantity { get; set; }
    }
}

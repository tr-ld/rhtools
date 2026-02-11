using System;
using Newtonsoft.Json;

namespace rhapi.Poco
{
    [Serializable]
    public class RhHolding
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("asset_code")]
        public string AssetCode { get; set; }

        [JsonProperty("total_quantity")]
        public decimal TotalQuantity { get; set; }

        [JsonProperty("quantity_available_for_trading")]
        public decimal QuantityAvailableForTrading { get; set; } 
    }
}

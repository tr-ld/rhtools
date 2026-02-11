using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhAccountResponse
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("buying_power")]
        public decimal BuyingPower { get; set; }

        [JsonProperty("buying_power_currency")]
        public string BuyingPowerCurrency { get; set; }
    }
}

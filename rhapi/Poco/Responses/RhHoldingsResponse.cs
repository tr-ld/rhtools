using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhHoldingsResponse : RhPagingResponse
    {
        [JsonProperty("results")]
        public RhHolding[] Results { get; set; }
    }
}

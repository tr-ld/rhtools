using System;
using Newtonsoft.Json;
using rhapi.Poco.Order;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhOrdersResponse : RhPagingResponse
    {
        [JsonProperty("results")]
        public RhOrder[] Results { get; set; }
    }
}

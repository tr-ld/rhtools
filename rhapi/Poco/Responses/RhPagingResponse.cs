using System;
using Newtonsoft.Json;

namespace rhapi.Poco.Responses
{
    [Serializable]
    public class RhPagingResponse
    {
        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("previous")]
        public string Previous { get; set; }
    }
}

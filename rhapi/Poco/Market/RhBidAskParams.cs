using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace rhapi.Poco.Market
{
    [Serializable]
    public class RhBidAskParams : QueryParams
    {
        [JsonProperty("symbol")]
        public string Symbol { get; init; }

        protected override IEnumerable<string> GetCustomParams()
        { return Symbol.Split(',').Select(s => $"symbol={s}"); }
    }
}

using Microsoft.AspNetCore.Mvc;
using rhapi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rhapi.Poco.Market
{
    [Serializable]
    public class RhEstimatedPriceParams : QueryParams
    {
        [FromQuery(Name = "symbol")]
        public string Symbol { get; init; }

        [FromQuery(Name = "side")]
        public EstimateSide Side { get; init; }

        [FromQuery(Name = "quantity")]
        public decimal[] Quantity { get; init; }

        protected override IEnumerable<string> GetCustomParams()
        { return [$"quantity={string.Join(',', Quantity.Select(q => $"{q}"))}", $"symbol={Symbol}"]; }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace rhapi.Poco.Market
{
    public class RhTradingPairsParams : QueryParams
    {
        [FromQuery(Name = "symbol")]
        public HashSet<(string crypto, string currency)> SymbolPairs { get; set; }

        public static ValueTask<RhTradingPairsParams> BindAsync(HttpContext context, ParameterInfo _)
        {
            var symbolPairs = new HashSet<(string crypto, string currency)>();
            var symbols = context.Request.Query["symbol"];
            
            foreach (var parts in symbols.Select(symbol => symbol.Split('-')))
            {
                if (parts.Length != 2) continue;

                symbolPairs.Add((parts[0], parts[1]));
            }

            var result = new RhTradingPairsParams { SymbolPairs = symbolPairs };
            return ValueTask.FromResult(result);
        }

        protected override IEnumerable<string> GetCustomParams()
        { return SymbolPairs.Select(pair => $"symbol={pair.crypto}-{pair.currency}"); }
    }
}

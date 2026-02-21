using rhdata;

namespace RHWebFront.Extensions;

public static class TradingPairExtensions
{
    /// <summary>
    /// Filters trading pairs to only those matching the specified currency
    /// </summary>
    public static IEnumerable<RHTradingPair> ForTradeCurrency(this IEnumerable<RHTradingPair> tradingPairs, string currency)
    {
        if (tradingPairs == null) return Enumerable.Empty<RHTradingPair>();
        if (string.IsNullOrEmpty(currency)) return tradingPairs;
        
        return tradingPairs.Where(p => p.Symbol.EndsWith($"-{currency}", StringComparison.OrdinalIgnoreCase));
    }
}
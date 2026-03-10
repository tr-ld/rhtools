namespace RHWebFront.Constants
{
    public static class CacheKeys
    {
        // BidAsk
        public const string BidAskPrefix = "BidAsk_";
        public static string BidAsk(string symbol) => $"{BidAskPrefix}{symbol}";

        // Asset Manager
        public const string Account = "Account";
        public const string HoldingsPrefix = "Holdings_";
        public const string Assets = "Assets";
        public const string AllOrders = "AllOrders";
        public const string TradingPairs = "TradingPairs";

        public static string Holdings(string[] symbols)
        {
            var symbolKey = symbols.CoalesceToAll();
            return $"{HoldingsPrefix}{symbolKey}";
        }
    }

    internal static class CacheKeysExtensions
    {
        internal static string CoalesceToAll(this string[] symbols)
            => (symbols == null || symbols.Length == 0) ? "__all" : string.Join(',', symbols.OrderBy(s => s, StringComparer.OrdinalIgnoreCase));
    }
}
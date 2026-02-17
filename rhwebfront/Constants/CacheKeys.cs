namespace RHWebFront.Constants;

public static class CacheKeys
{
    public const string BidAskPrefix = "BidAsk_";
    public static string BidAsk(string symbol) => $"{BidAskPrefix}{symbol}";
}
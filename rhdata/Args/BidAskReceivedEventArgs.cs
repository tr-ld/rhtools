namespace rhdata.Args;

public class BidAskReceivedEventArgs : EventArgs
{
    public required RHBidAsk[] BidAsks { get; init; }
    public DateTime ReceivedAt { get; init; }
}
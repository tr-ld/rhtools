using rhdata;

namespace RHWebFront.Models;

public class BidAskReceivedEventArgs : EventArgs
{
    public required RHBidAsk[] BidAsks { get; init; }
    public DateTime ReceivedAt { get; init; }
}
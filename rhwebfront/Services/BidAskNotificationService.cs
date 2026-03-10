using abstractions.Services;
using rhdata;
using rhdata.Args;

namespace RHWebFront.Services;

public class BidAskNotificationService : IBidAskNotificationService
{
    private readonly Lock _lock = new();
    
    public event EventHandler<BidAskReceivedEventArgs> BidAskReceived;

    public void NotifyBidAskReceived(RHBidAsk[] bidAsks, DateTime receivedAt)
    {
        lock (_lock)
        { BidAskReceived?.Invoke(this, new BidAskReceivedEventArgs { BidAsks = bidAsks, ReceivedAt = receivedAt }); }
    }
}
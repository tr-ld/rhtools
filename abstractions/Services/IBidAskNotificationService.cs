using rhdata;
using rhdata.Args;

namespace abstractions.Services;

public interface IBidAskNotificationService
{
    event EventHandler<BidAskReceivedEventArgs> BidAskReceived;
    void NotifyBidAskReceived(RHBidAsk[] bidAsks, DateTime receivedAt);
}
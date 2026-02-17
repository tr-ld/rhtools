using RHWebFront.Models;
using rhdata;

namespace RHWebFront.Services;

public interface IBidAskNotificationService
{
    event EventHandler<BidAskReceivedEventArgs> BidAskReceived;
    void NotifyBidAskReceived(RHBidAsk[] bidAsks, DateTime receivedAt);
}
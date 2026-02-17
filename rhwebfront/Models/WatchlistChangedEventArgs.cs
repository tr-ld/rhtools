namespace RHWebFront.Models;

public class WatchlistChangedEventArgs : EventArgs
{
    public required string[] Symbols { get; init; }
}
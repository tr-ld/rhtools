namespace rhdata.Args;

public class WatchlistChangedEventArgs : EventArgs
{
    public required string[] Symbols { get; init; }
}
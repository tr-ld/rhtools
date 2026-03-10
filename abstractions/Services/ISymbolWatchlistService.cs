using rhdata.Args;

namespace abstractions.Services;

public interface ISymbolWatchlistService
{
    Task<string[]> GetActiveSymbolsAsync();
    Task AddSymbolAsync(string symbol);
    Task RemoveSymbolAsync(string symbol);
    Task AddSymbolsAsync(string[] symbols);
    Task RemoveSymbolsAsync(string[] symbols);
    Task ReplaceSymbolsAsync(string[] symbols);

    event EventHandler<WatchlistChangedEventArgs> WatchlistChanged;
}
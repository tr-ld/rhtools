using rhdata;

namespace RHWebFront.Repositories;

public interface ISymbolWatchlistRepository
{
    Task<List<SymbolWatchlistEntry>> GetActiveSymbolsAsync(CancellationToken ct = default);
    Task<SymbolWatchlistEntry> GetBySymbolAsync(string symbol, CancellationToken ct = default);
    Task AddAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
    Task UpdateAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
    Task DeleteAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
}
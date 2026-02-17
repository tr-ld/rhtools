using rhdata;

namespace RHWebFront.Repositories;

public interface ISymbolWatchlistRepository
{
    Task<List<SymbolWatchlistEntry>> GetActiveSymbolsAsync(string currency = null, CancellationToken ct = default);
    Task<SymbolWatchlistEntry> GetBySymbolAndCurrencyAsync(string symbol, string currency, CancellationToken ct = default);
    Task AddAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
    Task UpdateAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
    Task DeleteAsync(SymbolWatchlistEntry entry, CancellationToken ct = default);
}
using rhdata;

namespace RHWebFront.Repositories.Emulated;

public class EmulatedSymbolWatchlistRepository : ISymbolWatchlistRepository
{
    private readonly List<SymbolWatchlistEntry> _watchlist =
    [
        new SymbolWatchlistEntry
        {
            Id = 1,
            Symbol = "BTC-USD",
            Currency = "USD",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            UpdatedAt = DateTime.UtcNow.AddDays(-5)
        },
        new SymbolWatchlistEntry
        {
            Id = 2,
            Symbol = "ETH-USD",
            Currency = "USD",
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-20),
            UpdatedAt = DateTime.UtcNow.AddDays(-3)
        },
        new SymbolWatchlistEntry
        {
            Id = 3,
            Symbol = "DOGE-USD",
            Currency = "USD",
            IsActive = false,
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        }
    ];

    public Task<List<SymbolWatchlistEntry>> GetActiveSymbolsAsync(string currency = null, CancellationToken ct = default)
    {
        var query = _watchlist.Where(s => s.IsActive);
        if (!string.IsNullOrEmpty(currency)) query = query.Where(s => s.Currency == currency);
        return Task.FromResult(query.OrderBy(s => s.Symbol).ToList());
    }

    public Task<SymbolWatchlistEntry> GetBySymbolAndCurrencyAsync(string symbol, string currency, CancellationToken ct = default)
    { return Task.FromResult(_watchlist.FirstOrDefault(s => s.Symbol == symbol && s.Currency == currency)); }

    public Task AddAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        entry.Id = _watchlist.Count > 0 ? _watchlist.Max(w => w.Id) + 1 : 1;
        entry.CreatedAt = DateTime.UtcNow;
        entry.UpdatedAt = DateTime.UtcNow;
        _watchlist.Add(entry);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        var existing = _watchlist.FirstOrDefault(w => w.Id == entry.Id);
        if (existing is not null)
        {
            existing.IsActive = entry.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        _watchlist.RemoveAll(w => w.Id == entry.Id);
        return Task.CompletedTask;
    }
}
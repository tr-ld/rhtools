using Microsoft.EntityFrameworkCore;
using RHWebFront.Data;
using rhdata;

namespace RHWebFront.Repositories;

public class SymbolWatchlistRepository(RhDbContext context) : ISymbolWatchlistRepository
{
    public async Task<List<SymbolWatchlistEntry>> GetActiveSymbolsAsync(string currency = null, CancellationToken ct = default)
    {
        var query = context.SymbolWatchlist.Where(s => s.IsActive);
        
        if (!string.IsNullOrEmpty(currency)) query = query.Where(s => s.Currency == currency);
        
        return await query.OrderBy(s => s.Symbol).ToListAsync(ct);
    }

    public async Task<SymbolWatchlistEntry> GetBySymbolAndCurrencyAsync(string symbol, string currency, CancellationToken ct = default)
    { return await context.SymbolWatchlist.FirstOrDefaultAsync(s => s.Symbol == symbol && s.Currency == currency, ct); }

    public async Task AddAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        context.SymbolWatchlist.Add(entry);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        context.SymbolWatchlist.Update(entry);
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(SymbolWatchlistEntry entry, CancellationToken ct = default)
    {
        context.SymbolWatchlist.Remove(entry);
        await context.SaveChangesAsync(ct);
    }
}
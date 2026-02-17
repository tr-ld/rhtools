using RHWebFront.Data;
using rhdata;

namespace RHWebFront.Repositories;

public class BidAskHistoryRepository(RhDbContext context) : IBidAskHistoryRepository
{
    public async Task AddAsync(BidAskHistoryEntry entry, CancellationToken ct = default)
    {
        context.BidAskHistory.Add(entry);
        await context.SaveChangesAsync(ct);
    }

    public async Task AddRangeAsync(IEnumerable<BidAskHistoryEntry> entries, CancellationToken ct = default)
    {
        context.BidAskHistory.AddRange(entries);
        await context.SaveChangesAsync(ct);
    }
}
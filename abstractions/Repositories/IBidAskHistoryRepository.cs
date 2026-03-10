using rhdata;

namespace abstractions.Repositories;

public interface IBidAskHistoryRepository
{
    Task AddAsync(BidAskHistoryEntry entry, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<BidAskHistoryEntry> entries, CancellationToken ct = default);
}
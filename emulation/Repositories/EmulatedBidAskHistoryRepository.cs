using abstractions.Repositories;
using rhdata;

namespace emulation.Repositories;

public class EmulatedBidAskHistoryRepository : IBidAskHistoryRepository
{
    private readonly List<BidAskHistoryEntry> _history = [];

    public Task AddAsync(BidAskHistoryEntry entry, CancellationToken ct = default)
    {
        entry.Id = _history.Count > 0 ? _history.Max(h => h.Id) + 1 : 1;
        _history.Add(entry);
        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<BidAskHistoryEntry> entries, CancellationToken ct = default)
    {
        var nextId = _history.Count > 0 ? _history.Max(h => h.Id) + 1 : 1;
        foreach (var entry in entries)
        {
            entry.Id = nextId++;
            _history.Add(entry);
        }
        return Task.CompletedTask;
    }
}
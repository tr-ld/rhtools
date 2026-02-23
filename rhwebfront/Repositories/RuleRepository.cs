using Microsoft.EntityFrameworkCore;
using rhdata.Rules;
using RHWebFront.Data;

namespace RHWebFront.Repositories;

public class RuleRepository(RhDbContext context) : IRuleRepository
{
    public async Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default)
    {
        return await context.RuleSets
            .Include(rs => rs.Rules).ThenInclude(r => r.Position)
            .Include(rs => rs.Rules).ThenInclude(r => r.Trigger)
            .Include(rs => rs.Rules).ThenInclude(r => r.Action)
            .Include(rs => rs.Rules).ThenInclude(r => r.Precision)
            .Include(rs => rs.Rules).ThenInclude(r => r.Amount)
            .OrderBy(rs => rs.Symbol)
            .ToListAsync(ct);
    }

    public async Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default)
    {
        return await context.RuleSets
            .Include(rs => rs.Rules).ThenInclude(r => r.Position)
            .Include(rs => rs.Rules).ThenInclude(r => r.Trigger)
            .Include(rs => rs.Rules).ThenInclude(r => r.Action)
            .Include(rs => rs.Rules).ThenInclude(r => r.Precision)
            .Include(rs => rs.Rules).ThenInclude(r => r.Amount)
            .FirstOrDefaultAsync(rs => rs.Symbol == symbol, ct);
    }

    public async Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency)
    {
        return await context.RuleSets.Where(rs => rs.Symbol.EndsWith($"-{tradeCurrency}")).ToListAsync();
    }
}
using abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using rhdata.Rules;
using RHWebFront.Data;

namespace RHWebFront.Repositories;

public class RuleRepository(RhDbContext context) : IRuleRepository
{
    public async Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default)
    {
        return await context.RuleSets
            .Include(rs => rs.Rules)
            .Include(rs => rs.Rules).ThenInclude(r => r.Trigger).ThenInclude(t => t.TriggerTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Action).ThenInclude(a => a.ActionTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Periodicity).ThenInclude(p => p.PeriodicityTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Amount).ThenInclude(a => a.AmountTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Price).ThenInclude(p => p.PriceTemplate)
            .OrderBy(rs => rs.Symbol)
            .ToListAsync(ct);
    }

    public async Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default)
    {
        return await context.RuleSets
            .Include(rs => rs.Rules)
            .Include(rs => rs.Rules).ThenInclude(r => r.Trigger).ThenInclude(t => t.TriggerTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Action).ThenInclude(a => a.ActionTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Periodicity).ThenInclude(p => p.PeriodicityTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Amount).ThenInclude(a => a.AmountTemplate)
            .Include(rs => rs.Rules).ThenInclude(r => r.Price).ThenInclude(p => p.PriceTemplate)
            .FirstOrDefaultAsync(rs => rs.Symbol == symbol, ct);
    }

    public async Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency)
    {
        return await context.RuleSets.Where(rs => rs.Symbol.EndsWith($"-{tradeCurrency}")).ToListAsync();
    }

    public async Task<List<TriggerTemplate>> GetTriggerTemplatesAsync(CancellationToken ct = default) => await context.TriggerTemplates.OrderBy(t => t.Name).ToListAsync(ct);

    public async Task<List<ActionTemplate>> GetActionTemplatesAsync(CancellationToken ct = default) => await context.ActionTemplates.OrderBy(a => a.Name).ToListAsync(ct);

    public async Task<List<PeriodicityTemplate>> GetPeriodicityTemplatesAsync(CancellationToken ct = default) => await context.PeriodicityTemplates.OrderBy(p => p.Name).ToListAsync(ct);

    public async Task<List<AmountTemplate>> GetAmountTemplatesAsync(CancellationToken ct = default) => await context.AmountTemplates.OrderBy(a => a.Name).ToListAsync(ct);

    public async Task<List<PriceTemplate>> GetPriceTemplatesAsync(CancellationToken ct = default) => await context.PriceTemplates.OrderBy(p => p.Name).ToListAsync(ct);

    public async Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet, CancellationToken ct = default)
    {
        if (0 == ruleSet.Id)
        {
            ruleSet.CreatedAt = DateTimeOffset.UtcNow;
            ruleSet.UpdatedAt = DateTimeOffset.UtcNow;
            context.RuleSets.Add(ruleSet);
        }
        else
        {
            ruleSet.UpdatedAt = DateTimeOffset.UtcNow;
            context.RuleSets.Update(ruleSet);
        }

        await context.SaveChangesAsync(ct);
        return ruleSet;
    }

    public async Task<Rule> SaveRuleAsync(Rule rule, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        if (0 == rule.Id)
        {
            rule.CreatedAt = now;
            rule.UpdatedAt = now;

            rule.Trigger?.CreatedAt = now;
            rule.Action?.CreatedAt = now;
            rule.Periodicity?.CreatedAt = now;
            rule.Amount?.CreatedAt = now;
            rule.Price?.CreatedAt = now;

            context.Rules.Add(rule);
        }
        else
        {
            rule.UpdatedAt = now;

            rule.Trigger?.UpdatedAt = now;
            rule.Action?.UpdatedAt = now;
            rule.Periodicity?.UpdatedAt = now;
            rule.Amount?.UpdatedAt = now;
            rule.Price?.UpdatedAt = now;

            context.Rules.Update(rule);
        }

        await context.SaveChangesAsync(ct);
        return rule;
    }
}
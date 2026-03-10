using abstractions.Repositories;
using rhdata.Rules;

namespace emulation.Repositories;

public class EmulatedRuleRepository : IRuleRepository
{
    private readonly List<RuleSet> _ruleSets = EmulatedRuleData.RuleSets;
    private readonly List<TriggerTemplate> _triggerTemplates = EmulatedRuleData.TriggerTemplates;
    private readonly List<ActionTemplate> _actionTemplates = EmulatedRuleData.ActionTemplates;
    private readonly List<PeriodicityTemplate> _periodicityTemplates = EmulatedRuleData.PeriodicityTemplates;
    private readonly List<AmountTemplate> _amountTemplates = EmulatedRuleData.AmountTemplates;
    private readonly List<PriceTemplate> _priceTemplates = EmulatedRuleData.PriceTemplates;
    private int _nextRuleSetId = 4;
    private int _nextRuleId = 5;
    private int _nextTriggerId = 5;
    private int _nextActionId = 5;
    private int _nextPeriodicityId = 5;
    private int _nextAmountId = 5;
    private int _nextPriceId = 5;

    public Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_ruleSets.ToList());
    }

    public Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default)
    {
        return Task.FromResult(_ruleSets.FirstOrDefault(rs => rs.Symbol == symbol));
    }

    public Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency)
    {
        var filtered = _ruleSets.Where(rs => rs.Symbol.EndsWith($"-{tradeCurrency}")).ToList();
        return Task.FromResult(filtered);
    }

    public Task<List<TriggerTemplate>> GetTriggerTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_triggerTemplates.ToList());
    }

    public Task<List<ActionTemplate>> GetActionTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_actionTemplates.ToList());
    }

    public Task<List<PeriodicityTemplate>> GetPeriodicityTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_periodicityTemplates.ToList());
    }

    public Task<List<AmountTemplate>> GetAmountTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_amountTemplates.ToList());
    }

    public Task<List<PriceTemplate>> GetPriceTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_priceTemplates.ToList());
    }

    public Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        if (ruleSet.Id == 0)
        {
            ruleSet.Id = _nextRuleSetId++;
            ruleSet.CreatedAt = now;
            ruleSet.UpdatedAt = now;
            _ruleSets.Add(ruleSet);
        }
        else
        {
            ruleSet.UpdatedAt = now;
            var existing = _ruleSets.FirstOrDefault(rs => rs.Id == ruleSet.Id);
            if (existing is not null)
            {
                var index = _ruleSets.IndexOf(existing);
                _ruleSets[index] = ruleSet;
            }
        }

        return Task.FromResult(ruleSet);
    }

    public Task<Rule> SaveRuleAsync(Rule rule, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        if (rule.Id == 0)
        {
            rule.Id = _nextRuleId++;
            rule.CreatedAt = now;
            rule.UpdatedAt = now;

            if (rule.Trigger is not null && rule.Trigger.Id == 0)
            {
                rule.Trigger.Id = _nextTriggerId++;
                rule.TriggerId = rule.Trigger.Id;
                rule.Trigger.CreatedAt = now;
                rule.Trigger.UpdatedAt = now;
                rule.Trigger.TriggerTemplate = _triggerTemplates.FirstOrDefault(t => t.Id == rule.Trigger.TriggerTemplateId);
            }

            if (rule.Action is not null && rule.Action.Id == 0)
            {
                rule.Action.Id = _nextActionId++;
                rule.ActionId = rule.Action.Id;
                rule.Action.CreatedAt = now;
                rule.Action.UpdatedAt = now;
                rule.Action.ActionTemplate = _actionTemplates.FirstOrDefault(a => a.Id == rule.Action.ActionTemplateId);
            }

            if (rule.Periodicity is not null && rule.Periodicity.Id == 0)
            {
                rule.Periodicity.Id = _nextPeriodicityId++;
                rule.PeriodicityId = rule.Periodicity.Id;
                rule.Periodicity.CreatedAt = now;
                rule.Periodicity.UpdatedAt = now;
                rule.Periodicity.PeriodicityTemplate = _periodicityTemplates.FirstOrDefault(p => p.Id == rule.Periodicity.PeriodicityTemplateId);
            }

            if (rule.Amount is not null && rule.Amount.Id == 0)
            {
                rule.Amount.Id = _nextAmountId++;
                rule.AmountId = rule.Amount.Id;
                rule.Amount.CreatedAt = now;
                rule.Amount.UpdatedAt = now;
                rule.Amount.AmountTemplate = _amountTemplates.FirstOrDefault(a => a.Id == rule.Amount.AmountTemplateId);
            }

            if (rule.Price is not null && rule.Price.Id == 0)
            {
                rule.Price.Id = _nextPriceId++;
                rule.PriceId = rule.Price.Id;
                rule.Price.CreatedAt = now;
                rule.Price.UpdatedAt = now;
                rule.Price.PriceTemplate = _priceTemplates.FirstOrDefault(p => p.Id == rule.Price.PriceTemplateId);
            }
        }
        else
        {
            rule.UpdatedAt = now;

            rule.Trigger?.UpdatedAt = now;
            rule.Action?.UpdatedAt = now;
            rule.Periodicity?.UpdatedAt = now;
            rule.Amount?.UpdatedAt = now;
            rule.Price?.UpdatedAt = now;

            var ruleSet = _ruleSets.FirstOrDefault(rs => rs.Id == rule.RuleSetId);
            if (ruleSet is null) return Task.FromResult(rule);
            
            var existingRule = ruleSet.Rules.FirstOrDefault(r => r.Id == rule.Id);
            if (existingRule is null) return Task.FromResult(rule);
                
            var index = ruleSet.Rules.IndexOf(existingRule);
            ruleSet.Rules[index] = rule;
        }

        return Task.FromResult(rule);
    }
}
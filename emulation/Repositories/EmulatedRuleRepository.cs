using abstractions.Repositories;
using rhdata.Rules;

namespace emulation.Repositories;

public class EmulatedRuleRepository : IRuleRepository
{
    private readonly List<RuleSet> _ruleSets = EmulatedRuleData.RuleSets;
    private readonly List<TriggerTemplate> _triggerTemplates = EmulatedRuleData.TriggerTemplates;
    private readonly List<ActionTemplate> _actionTemplates = EmulatedRuleData.ActionTemplates;
    private readonly List<PrecisionTemplate> _precisionTemplates = EmulatedRuleData.PrecisionTemplates;
    private readonly List<AmountTemplate> _amountTemplates = EmulatedRuleData.AmountTemplates;
    private int _nextRuleSetId = 4;
    private int _nextRuleId = 5;
    private int _nextTriggerId = 5;
    private int _nextActionId = 5;
    private int _nextPrecisionId = 5;
    private int _nextAmountId = 5;

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

    public Task<List<PrecisionTemplate>> GetPrecisionTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_precisionTemplates.ToList());
    }

    public Task<List<AmountTemplate>> GetAmountTemplatesAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_amountTemplates.ToList());
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

            if (rule.Precision is not null && rule.Precision.Id == 0)
            {
                rule.Precision.Id = _nextPrecisionId++;
                rule.PrecisionId = rule.Precision.Id;
                rule.Precision.CreatedAt = now;
                rule.Precision.UpdatedAt = now;
                rule.Precision.PrecisionTemplate = _precisionTemplates.FirstOrDefault(p => p.Id == rule.Precision.PrecisionTemplateId);
            }

            if (rule.Amount is not null && rule.Amount.Id == 0)
            {
                rule.Amount.Id = _nextAmountId++;
                rule.AmountId = rule.Amount.Id;
                rule.Amount.CreatedAt = now;
                rule.Amount.UpdatedAt = now;
                rule.Amount.AmountTemplate = _amountTemplates.FirstOrDefault(a => a.Id == rule.Amount.AmountTemplateId);
            }
        }
        else
        {
            rule.UpdatedAt = now;

            rule.Trigger?.UpdatedAt = now;
            rule.Action?.UpdatedAt = now;
            rule.Precision?.UpdatedAt = now;
            rule.Amount?.UpdatedAt = now;

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
using abstractions.Repositories;
using rhdata.Rules;

namespace emulation.Repositories;

public class EmulatedRuleRepository : IRuleRepository
{
    private readonly List<RuleSet> _ruleSets;
    private readonly List<TriggerTemplate> _triggerTemplates;
    private readonly List<ActionTemplate> _actionTemplates;
    private readonly List<PrecisionTemplate> _precisionTemplates;
    private readonly List<AmountTemplate> _amountTemplates;
    private int _nextRuleSetId = 4;
    private int _nextRuleId = 5;
    private int _nextTriggerId = 5;
    private int _nextActionId = 5;
    private int _nextPrecisionId = 5;
    private int _nextAmountId = 5;

    public EmulatedRuleRepository()
    {
        var now = DateTimeOffset.UtcNow;

        // Create template data
        var triggerTemplate = new TriggerTemplate
        {
            Id = 1,
            Name = "Price Above",
            Description = "Triggers when price goes above specified value",
            CreatedAt = now,
            UpdatedAt = now
        };

        var actionTemplate = new ActionTemplate
        {
            Id = 1,
            Name = "Market Buy",
            Description = "Execute a market buy order",
            CreatedAt = now,
            UpdatedAt = now
        };

        var precisionTemplate = new PrecisionTemplate
        {
            Id = 1,
            Name = "Fixed Precision",
            Description = "Use a fixed decimal precision",
            CreatedAt = now,
            UpdatedAt = now
        };

        var amountTemplate = new AmountTemplate
        {
            Id = 1,
            Name = "Fixed Amount",
            Description = "Use a fixed amount",
            CreatedAt = now,
            UpdatedAt = now
        };

        _triggerTemplates = [triggerTemplate];
        _actionTemplates = [actionTemplate];
        _precisionTemplates = [precisionTemplate];
        _amountTemplates = [amountTemplate];

        // Create rule sets with full object graphs
        _ruleSets =
        [
            new RuleSet
            {
                Id = 1,
                Symbol = "BTC-USD",
                CreatedAt = now.AddDays(-10),
                UpdatedAt = now.AddDays(-1),
                Rules =
                [
                    new Rule
                    {
                        Id = 1,
                        RuleSetId = 1,
                        Position = 1,
                        TriggerId = 1,
                        ActionId = 1,
                        PrecisionId = 1,
                        AmountId = 1,
                        IsActive = true,
                        CreatedAt = now.AddDays(-10),
                        UpdatedAt = now.AddDays(-1),
                        Trigger = new RuleTrigger
                        {
                            Id = 1,
                            TriggerTemplateId = 1,
                            Value = 50000.00m,
                            TriggerTemplate = triggerTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Action = new RuleAction
                        {
                            Id = 1,
                            ActionTemplateId = 1,
                            Value = 100.00m,
                            ActionTemplate = actionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Precision = new RulePrecision
                        {
                            Id = 1,
                            PrecisionTemplateId = 1,
                            Value = 2,
                            PrecisionTemplate = precisionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Amount = new RuleAmount
                        {
                            Id = 1,
                            AmountTemplateId = 1,
                            Value = 100.00m,
                            AmountTemplate = amountTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        }
                    }
                ]
            },
            new RuleSet
            {
                Id = 2,
                Symbol = "ETH-USD",
                CreatedAt = now.AddDays(-5),
                UpdatedAt = now,
                Rules =
                [
                    new Rule
                    {
                        Id = 2,
                        RuleSetId = 2,
                        Position = 1,
                        TriggerId = 2,
                        ActionId = 2,
                        PrecisionId = 2,
                        AmountId = 2,
                        IsActive = true,
                        CreatedAt = now.AddDays(-5),
                        UpdatedAt = now,
                        Trigger = new RuleTrigger
                        {
                            Id = 2,
                            TriggerTemplateId = 1,
                            Value = 3000.00m,
                            TriggerTemplate = triggerTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Action = new RuleAction
                        {
                            Id = 2,
                            ActionTemplateId = 1,
                            Value = 50.00m,
                            ActionTemplate = actionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Precision = new RulePrecision
                        {
                            Id = 2,
                            PrecisionTemplateId = 1,
                            Value = 4,
                            PrecisionTemplate = precisionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Amount = new RuleAmount
                        {
                            Id = 2,
                            AmountTemplateId = 1,
                            Value = 50.00m,
                            AmountTemplate = amountTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        }
                    },
                    new Rule
                    {
                        Id = 3,
                        RuleSetId = 2,
                        Position = 2,
                        TriggerId = 3,
                        ActionId = 3,
                        PrecisionId = 3,
                        AmountId = 3,
                        IsActive = false,
                        CreatedAt = now.AddDays(-5),
                        UpdatedAt = now,
                        Trigger = new RuleTrigger
                        {
                            Id = 3,
                            TriggerTemplateId = 1,
                            Value = 2500.00m,
                            TriggerTemplate = triggerTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Action = new RuleAction
                        {
                            Id = 3,
                            ActionTemplateId = 1,
                            Value = 25.00m,
                            ActionTemplate = actionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Precision = new RulePrecision
                        {
                            Id = 3,
                            PrecisionTemplateId = 1,
                            Value = 4,
                            PrecisionTemplate = precisionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Amount = new RuleAmount
                        {
                            Id = 3,
                            AmountTemplateId = 1,
                            Value = 25.00m,
                            AmountTemplate = amountTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        }
                    }
                ]
            },
            new RuleSet
            {
                Id = 3,
                Symbol = "DOGE-USD",
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now,
                Rules =
                [
                    new Rule
                    {
                        Id = 4,
                        RuleSetId = 3,
                        Position = 1,
                        TriggerId = 4,
                        ActionId = 4,
                        PrecisionId = 4,
                        AmountId = 4,
                        IsActive = true,
                        CreatedAt = now.AddDays(-2),
                        UpdatedAt = now,
                        Trigger = new RuleTrigger
                        {
                            Id = 4,
                            TriggerTemplateId = 1,
                            Value = 0.15m,
                            TriggerTemplate = triggerTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Action = new RuleAction
                        {
                            Id = 4,
                            ActionTemplateId = 1,
                            Value = 10.00m,
                            ActionTemplate = actionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Precision = new RulePrecision
                        {
                            Id = 4,
                            PrecisionTemplateId = 1,
                            Value = 6,
                            PrecisionTemplate = precisionTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        },
                        Amount = new RuleAmount
                        {
                            Id = 4,
                            AmountTemplateId = 1,
                            Value = 10.00m,
                            AmountTemplate = amountTemplate,
                            CreatedAt = now,
                            UpdatedAt = now
                        }
                    }
                ]
            }
        ];
    }

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
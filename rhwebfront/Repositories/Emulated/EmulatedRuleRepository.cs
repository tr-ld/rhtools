using rhdata.Rules;

namespace RHWebFront.Repositories.Emulated;

public class EmulatedRuleRepository : IRuleRepository
{
    private readonly List<RuleSet> _ruleSets;

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
                        PositionId = 1,
                        TriggerId = 1,
                        ActionId = 1,
                        PrecisionId = 1,
                        AmountId = 1,
                        IsActive = true,
                        CreatedAt = now.AddDays(-10),
                        UpdatedAt = now.AddDays(-1),
                        Position = new RuleOrderPosition { Id = 1, Position = 1, CreatedAt = now, UpdatedAt = now },
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
                        PositionId = 2,
                        TriggerId = 2,
                        ActionId = 2,
                        PrecisionId = 2,
                        AmountId = 2,
                        IsActive = true,
                        CreatedAt = now.AddDays(-5),
                        UpdatedAt = now,
                        Position = new RuleOrderPosition { Id = 2, Position = 1, CreatedAt = now, UpdatedAt = now },
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
                        PositionId = 3,
                        TriggerId = 3,
                        ActionId = 3,
                        PrecisionId = 3,
                        AmountId = 3,
                        IsActive = false,
                        CreatedAt = now.AddDays(-5),
                        UpdatedAt = now,
                        Position = new RuleOrderPosition { Id = 3, Position = 2, CreatedAt = now, UpdatedAt = now },
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
                        PositionId = 4,
                        TriggerId = 4,
                        ActionId = 4,
                        PrecisionId = 4,
                        AmountId = 4,
                        IsActive = true,
                        CreatedAt = now.AddDays(-2),
                        UpdatedAt = now,
                        Position = new RuleOrderPosition { Id = 4, Position = 1, CreatedAt = now, UpdatedAt = now },
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
    { return Task.FromResult(_ruleSets.ToList()); }

    public Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default)
    { return Task.FromResult(_ruleSets.FirstOrDefault(rs => rs.Symbol == symbol)); }

    public Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency)
    {
        var filtered = _ruleSets.Where(rs => rs.Symbol.EndsWith($"-{tradeCurrency}")).ToList();
        return Task.FromResult(filtered);
    }
}
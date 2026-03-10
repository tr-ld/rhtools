using rhdata.Rules;

namespace emulation.Repositories;

public static class EmulatedRuleData
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    public static List<TriggerTemplate> TriggerTemplates { get; } =
    [
        new TriggerTemplate { Id = 1, Name = "Down Percent", Description = "Triggers when price decreases by a percentage", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 2, Name = "Up Percent", Description = "Triggers when price increases by a percentage", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 3, Name = "Down Flat", Description = "Triggers when price decreases by a flat amount", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 4, Name = "Up Flat", Description = "Triggers when price increases by a flat amount", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<ActionTemplate> ActionTemplates { get; } =
    [
        new ActionTemplate { Id = 1, Name = "Limit Sell", Description = "Sell order at a specific price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 2, Name = "Limit Buy", Description = "Buy order at a specific price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 3, Name = "Market Sell", Description = "Market sell order executed immediately at current market price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 4, Name = "Market Buy", Description = "Market buy order executed immediately at current market price", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<PeriodicityTemplate> PeriodicityTemplates { get; } =
    [
        new PeriodicityTemplate { Id = 1, Name = "Seconds", Description = "Evaluate rule every N seconds", CreatedAt = Now, UpdatedAt = Now },
        new PeriodicityTemplate { Id = 2, Name = "Minutes", Description = "Evaluate rule every N minutes", CreatedAt = Now, UpdatedAt = Now },
        new PeriodicityTemplate { Id = 3, Name = "Hours", Description = "Evaluate rule every N hours", CreatedAt = Now, UpdatedAt = Now },
        new PeriodicityTemplate { Id = 4, Name = "Days", Description = "Evaluate rule every N days", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<AmountTemplate> AmountTemplates { get; } =
    [
        new AmountTemplate { Id = 1, Name = "Flat", Description = "Specific quantity of the asset", CreatedAt = Now, UpdatedAt = Now },
        new AmountTemplate { Id = 2, Name = "Percent", Description = "Percentage of available holdings", CreatedAt = Now, UpdatedAt = Now },
        new AmountTemplate { Id = 3, Name = "Currency", Description = "Specific amount of holdings in currency", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<PriceTemplate> PriceTemplates { get; } =
    [
        new PriceTemplate { Id = 1, Name = "Flat", Description = "Specific price point", CreatedAt = Now, UpdatedAt = Now },
        new PriceTemplate { Id = 2, Name = "Percent From Create", Description = "Percent offset from market price at rule activation", CreatedAt = Now, UpdatedAt = Now },
        new PriceTemplate { Id = 3, Name = "Percent From Execute", Description = "Percent offset from market price at trigger execution", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<RuleSet> RuleSets { get; } =
    [
        new RuleSet
        {
            Id = 1,
            Symbol = "BTC-USD",
            CreatedAt = Now.AddDays(-10),
            UpdatedAt = Now.AddDays(-1),
            Rules =
            [
                new Rule
                {
                    Id = 1,
                    RuleSetId = 1,
                    Position = 1,
                    TriggerId = 1,
                    ActionId = 1,
                    PeriodicityId = 1,
                    AmountId = 1,
                    PriceId = 1,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-10),
                    UpdatedAt = Now.AddDays(-1),
                    Trigger = new RuleTrigger { Id = 1, TriggerTemplateId = 1, Value = 50000.00m, TriggerTemplate = TriggerTemplates[0], CreatedAt = Now, UpdatedAt = Now },
                    Action = new RuleAction { Id = 1, ActionTemplateId = 3, Value = 100.00m, ActionTemplate = ActionTemplates[2], CreatedAt = Now, UpdatedAt = Now },
                    Periodicity = new RulePeriodicity { Id = 1, PeriodicityTemplateId = 2, Value = 2, PeriodicityTemplate = PeriodicityTemplates[1], CreatedAt = Now, UpdatedAt = Now },
                    Amount = new RuleAmount { Id = 1, AmountTemplateId = 1, Value = 100.00m, AmountTemplate = AmountTemplates[0], CreatedAt = Now, UpdatedAt = Now },
                    Price = new RulePrice { Id = 1, PriceTemplateId = 1, Value = 0m, PriceTemplate = PriceTemplates[0], CreatedAt = Now, UpdatedAt = Now }
                }
            ]
        },
        new RuleSet
        {
            Id = 2,
            Symbol = "ETH-USD",
            CreatedAt = Now.AddDays(-5),
            UpdatedAt = Now,
            Rules =
            [
                new Rule
                {
                    Id = 2,
                    RuleSetId = 2,
                    Position = 1,
                    TriggerId = 2,
                    ActionId = 2,
                    PeriodicityId = 2,
                    AmountId = 2,
                    PriceId = 2,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-5),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger { Id = 2, TriggerTemplateId = 2, Value = 3000.00m, TriggerTemplate = TriggerTemplates[1], CreatedAt = Now, UpdatedAt = Now },
                    Action = new RuleAction { Id = 2, ActionTemplateId = 4, Value = 50.00m, ActionTemplate = ActionTemplates[3], CreatedAt = Now, UpdatedAt = Now },
                    Periodicity = new RulePeriodicity { Id = 2, PeriodicityTemplateId = 2, Value = 4, PeriodicityTemplate = PeriodicityTemplates[1], CreatedAt = Now, UpdatedAt = Now },
                    Amount = new RuleAmount { Id = 2, AmountTemplateId = 2, Value = 50.00m, AmountTemplate = AmountTemplates[1], CreatedAt = Now, UpdatedAt = Now },
                    Price = new RulePrice { Id = 2, PriceTemplateId = 1, Value = 0m, PriceTemplate = PriceTemplates[0], CreatedAt = Now, UpdatedAt = Now }
                },
                new Rule
                {
                    Id = 3,
                    RuleSetId = 2,
                    Position = 2,
                    TriggerId = 3,
                    ActionId = 3,
                    PeriodicityId = 3,
                    AmountId = 3,
                    PriceId = 3,
                    IsActive = false,
                    CreatedAt = Now.AddDays(-5),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger { Id = 3, TriggerTemplateId = 3, Value = 2500.00m, TriggerTemplate = TriggerTemplates[2], CreatedAt = Now, UpdatedAt = Now },
                    Action = new RuleAction { Id = 3, ActionTemplateId = 1, Value = 25.00m, ActionTemplate = ActionTemplates[0], CreatedAt = Now, UpdatedAt = Now },
                    Periodicity = new RulePeriodicity { Id = 3, PeriodicityTemplateId = 3, Value = 4, PeriodicityTemplate = PeriodicityTemplates[2], CreatedAt = Now, UpdatedAt = Now },
                    Amount = new RuleAmount { Id = 3, AmountTemplateId = 3, Value = 25.00m, AmountTemplate = AmountTemplates[2], CreatedAt = Now, UpdatedAt = Now },
                    Price = new RulePrice { Id = 3, PriceTemplateId = 3, Value = 2800.00m, PriceTemplate = PriceTemplates[2], CreatedAt = Now, UpdatedAt = Now }
                }
            ]
        },
        new RuleSet
        {
            Id = 3,
            Symbol = "DOGE-USD",
            CreatedAt = Now.AddDays(-2),
            UpdatedAt = Now,
            Rules =
            [
                new Rule
                {
                    Id = 4,
                    RuleSetId = 3,
                    Position = 1,
                    TriggerId = 4,
                    ActionId = 4,
                    PeriodicityId = 4,
                    AmountId = 4,
                    PriceId = 4,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-2),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger { Id = 4, TriggerTemplateId = 4, Value = 0.15m, TriggerTemplate = TriggerTemplates[3], CreatedAt = Now, UpdatedAt = Now },
                    Action = new RuleAction { Id = 4, ActionTemplateId = 2, Value = 10.00m, ActionTemplate = ActionTemplates[1], CreatedAt = Now, UpdatedAt = Now },
                    Periodicity = new RulePeriodicity { Id = 4, PeriodicityTemplateId = 1, Value = 6, PeriodicityTemplate = PeriodicityTemplates[0], CreatedAt = Now, UpdatedAt = Now },
                    Amount = new RuleAmount { Id = 4, AmountTemplateId = 1, Value = 10.00m, AmountTemplate = AmountTemplates[0], CreatedAt = Now, UpdatedAt = Now },
                    Price = new RulePrice { Id = 4, PriceTemplateId = 3, Value = -2.50m, PriceTemplate = PriceTemplates[2], CreatedAt = Now, UpdatedAt = Now }
                }
            ]
        }
    ];
}
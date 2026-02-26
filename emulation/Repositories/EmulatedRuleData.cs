using rhdata.Rules;

namespace emulation.Repositories;

public static class EmulatedRuleData
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    public static List<TriggerTemplate> TriggerTemplates { get; } =
    [
        new TriggerTemplate { Id = 1, Name = "DownPercent", Description = "Triggers when price decreases by a percentage", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 2, Name = "UpPercent", Description = "Triggers when price increases by a percentage", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 3, Name = "DownAbsolute", Description = "Triggers when price decreases by an absolute amount", CreatedAt = Now, UpdatedAt = Now },
        new TriggerTemplate { Id = 4, Name = "UpAbsolute", Description = "Triggers when price increases by an absolute amount", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<ActionTemplate> ActionTemplates { get; } =
    [
        new ActionTemplate { Id = 1, Name = "LimitSellAbsolute", Description = "Limit sell order at absolute price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 2, Name = "LimitSellRelativeAtCreate", Description = "Limit sell order at price relative to rule creation", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 3, Name = "LimitSellRelativeAtExecute", Description = "Limit sell order at price relative to trigger execution", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 4, Name = "LimitBuyAbsolute", Description = "Limit buy order at absolute price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 5, Name = "LimitBuyRelativeAtCreate", Description = "Limit buy order at price relative to rule creation", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 6, Name = "LimitBuyRelativeAtExecute", Description = "Limit buy order at price relative to trigger execution", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 7, Name = "MarketSell", Description = "Market sell order executed immediately at current market price", CreatedAt = Now, UpdatedAt = Now },
        new ActionTemplate { Id = 8, Name = "MarketBuy", Description = "Market buy order executed immediately at current market price", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<PrecisionTemplate> PrecisionTemplates { get; } =
    [
        new PrecisionTemplate { Id = 1, Name = "Seconds", Description = "Evaluate rule every N seconds", CreatedAt = Now, UpdatedAt = Now },
        new PrecisionTemplate { Id = 2, Name = "Minutes", Description = "Evaluate rule every N minutes", CreatedAt = Now, UpdatedAt = Now },
        new PrecisionTemplate { Id = 3, Name = "Hours", Description = "Evaluate rule every N hours", CreatedAt = Now, UpdatedAt = Now },
        new PrecisionTemplate { Id = 4, Name = "Days", Description = "Evaluate rule every N days", CreatedAt = Now, UpdatedAt = Now }
    ];

    public static List<AmountTemplate> AmountTemplates { get; } =
    [
        new AmountTemplate { Id = 1, Name = "Absolute", Description = "Specific quantity of the asset", CreatedAt = Now, UpdatedAt = Now },
        new AmountTemplate { Id = 2, Name = "Percent", Description = "Percentage of available holdings", CreatedAt = Now, UpdatedAt = Now },
        new AmountTemplate { Id = 3, Name = "All", Description = "All available holdings", CreatedAt = Now, UpdatedAt = Now }
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
                    PrecisionId = 1,
                    AmountId = 1,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-10),
                    UpdatedAt = Now.AddDays(-1),
                    Trigger = new RuleTrigger
                    {
                        Id = 1,
                        TriggerTemplateId = 1,
                        Value = 50000.00m,
                        TriggerTemplate = TriggerTemplates[0],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Action = new RuleAction
                    {
                        Id = 1,
                        ActionTemplateId = 7,
                        Value = 100.00m,
                        ActionTemplate = ActionTemplates[6],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Precision = new RulePrecision
                    {
                        Id = 1,
                        PrecisionTemplateId = 2,
                        Value = 2,
                        PrecisionTemplate = PrecisionTemplates[1],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Amount = new RuleAmount
                    {
                        Id = 1,
                        AmountTemplateId = 1,
                        Value = 100.00m,
                        AmountTemplate = AmountTemplates[0],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    }
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
                    PrecisionId = 2,
                    AmountId = 2,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-5),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger
                    {
                        Id = 2,
                        TriggerTemplateId = 2,
                        Value = 3000.00m,
                        TriggerTemplate = TriggerTemplates[1],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Action = new RuleAction
                    {
                        Id = 2,
                        ActionTemplateId = 8,
                        Value = 50.00m,
                        ActionTemplate = ActionTemplates[7],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Precision = new RulePrecision
                    {
                        Id = 2,
                        PrecisionTemplateId = 2,
                        Value = 4,
                        PrecisionTemplate = PrecisionTemplates[1],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Amount = new RuleAmount
                    {
                        Id = 2,
                        AmountTemplateId = 2,
                        Value = 50.00m,
                        AmountTemplate = AmountTemplates[1],
                        CreatedAt = Now,
                        UpdatedAt = Now
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
                    CreatedAt = Now.AddDays(-5),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger
                    {
                        Id = 3,
                        TriggerTemplateId = 3,
                        Value = 2500.00m,
                        TriggerTemplate = TriggerTemplates[2],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Action = new RuleAction
                    {
                        Id = 3,
                        ActionTemplateId = 1,
                        Value = 25.00m,
                        ActionTemplate = ActionTemplates[0],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Precision = new RulePrecision
                    {
                        Id = 3,
                        PrecisionTemplateId = 3,
                        Value = 4,
                        PrecisionTemplate = PrecisionTemplates[2],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Amount = new RuleAmount
                    {
                        Id = 3,
                        AmountTemplateId = 3,
                        Value = 25.00m,
                        AmountTemplate = AmountTemplates[2],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    }
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
                    PrecisionId = 4,
                    AmountId = 4,
                    IsActive = true,
                    CreatedAt = Now.AddDays(-2),
                    UpdatedAt = Now,
                    Trigger = new RuleTrigger
                    {
                        Id = 4,
                        TriggerTemplateId = 4,
                        Value = 0.15m,
                        TriggerTemplate = TriggerTemplates[3],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Action = new RuleAction
                    {
                        Id = 4,
                        ActionTemplateId = 4,
                        Value = 10.00m,
                        ActionTemplate = ActionTemplates[3],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Precision = new RulePrecision
                    {
                        Id = 4,
                        PrecisionTemplateId = 1,
                        Value = 6,
                        PrecisionTemplate = PrecisionTemplates[0],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    },
                    Amount = new RuleAmount
                    {
                        Id = 4,
                        AmountTemplateId = 1,
                        Value = 10.00m,
                        AmountTemplate = AmountTemplates[0],
                        CreatedAt = Now,
                        UpdatedAt = Now
                    }
                }
            ]
        }
    ];
}
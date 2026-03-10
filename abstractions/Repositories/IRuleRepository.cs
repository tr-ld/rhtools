using rhdata.Rules;

namespace abstractions.Repositories;

public interface IRuleRepository
{
    Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default);
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default);
    Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency);
    
    Task<List<TriggerTemplate>> GetTriggerTemplatesAsync(CancellationToken ct = default);
    Task<List<ActionTemplate>> GetActionTemplatesAsync(CancellationToken ct = default);
    Task<List<PeriodicityTemplate>> GetPeriodicityTemplatesAsync(CancellationToken ct = default);
    Task<List<AmountTemplate>> GetAmountTemplatesAsync(CancellationToken ct = default);
    Task<List<PriceTemplate>> GetPriceTemplatesAsync(CancellationToken ct = default);
    
    Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet, CancellationToken ct = default);
    Task<Rule> SaveRuleAsync(Rule rule, CancellationToken ct = default);
}
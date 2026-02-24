using rhdata.Rules;

namespace RHWebFront.Repositories;

public interface IRuleRepository
{
    Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default);
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default);
    Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency);
    
    Task<List<TriggerTemplate>> GetTriggerTemplatesAsync(CancellationToken ct = default);
    Task<List<ActionTemplate>> GetActionTemplatesAsync(CancellationToken ct = default);
    Task<List<PrecisionTemplate>> GetPrecisionTemplatesAsync(CancellationToken ct = default);
    Task<List<AmountTemplate>> GetAmountTemplatesAsync(CancellationToken ct = default);
    
    Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet, CancellationToken ct = default);
    Task<Rule> SaveRuleAsync(Rule rule, CancellationToken ct = default);
}
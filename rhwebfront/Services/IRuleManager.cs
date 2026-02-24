using rhdata.Rules;

namespace RHWebFront.Services;

public interface IRuleManager
{
    Task<List<RuleSet>> GetRuleSetsForTradeCurrencyAsync();
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol);
    
    Task<List<TriggerTemplate>> GetTriggerTemplatesAsync();
    Task<List<ActionTemplate>> GetActionTemplatesAsync();
    Task<List<PrecisionTemplate>> GetPrecisionTemplatesAsync();
    Task<List<AmountTemplate>> GetAmountTemplatesAsync();
    
    Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet);
    Task<Rule> SaveRuleAsync(Rule rule);
}
using rhdata.Rules;

namespace abstractions.Services;

public interface IRuleManager
{
    RuleSet ActiveRuleSet { get; set; }
    Task<List<RuleSet>> GetRuleSetsForTradeCurrencyAsync();
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol);
    
    Task<List<TriggerTemplate>> GetTriggerTemplatesAsync();
    Task<List<ActionTemplate>> GetActionTemplatesAsync();
    Task<List<PeriodicityTemplate>> GetPeriodicityTemplatesAsync();
    Task<List<AmountTemplate>> GetAmountTemplatesAsync();
    Task<List<PriceTemplate>> GetPriceTemplatesAsync();
    
    Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet);
    Task<Rule> SaveRuleAsync(Rule rule);
    Task SaveActiveRuleAsync(int ruleId, Rule pendingRule);
}
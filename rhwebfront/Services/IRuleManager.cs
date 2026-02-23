using rhdata.Rules;

namespace RHWebFront.Services;

public interface IRuleManager
{
    Task<List<RuleSet>> GetRuleSetsForTradeCurrencyAsync();
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol);
}
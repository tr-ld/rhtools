using rhdata.Rules;

namespace RHWebFront.Repositories;

public interface IRuleRepository
{
    Task<List<RuleSet>> GetAllRuleSetsWithRelatedDataAsync(CancellationToken ct = default);
    Task<RuleSet> GetRuleSetBySymbolAsync(string symbol, CancellationToken ct = default);
    Task<List<RuleSet>> GetRuleSetsByCurrencyAsync(string tradeCurrency);
}
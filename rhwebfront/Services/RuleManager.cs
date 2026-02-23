using Microsoft.Extensions.Options;
using rhdata.Rules;
using RHWebFront.Config;
using RHWebFront.Repositories;

namespace RHWebFront.Services;

public class RuleManager(IRuleRepository repository, IOptionsSnapshot<AppConfig> appConfig, ILogger<RuleManager> logger) : IRuleManager
{
    public async Task<List<RuleSet>> GetRuleSetsForTradeCurrencyAsync()
    {
        try { return await repository.GetRuleSetsByCurrencyAsync(appConfig.Value.TradeCurrency); }
        catch (Exception ex) { logger.LogError(ex, "Error loading rule sets"); throw; }
    }

    public async Task<RuleSet> GetRuleSetBySymbolAsync(string symbol)
    {
        try { return await repository.GetRuleSetBySymbolAsync(symbol); }
        catch (Exception ex) { logger.LogError(ex, "Error loading rule set for symbol {Symbol}", symbol); throw; }
    }
}
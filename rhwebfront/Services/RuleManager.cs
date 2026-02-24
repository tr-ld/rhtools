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

    public async Task<List<TriggerTemplate>> GetTriggerTemplatesAsync()
    {
        try { return await repository.GetTriggerTemplatesAsync(); }
        catch (Exception ex) { logger.LogError(ex, "Error loading trigger templates"); throw; }
    }

    public async Task<List<ActionTemplate>> GetActionTemplatesAsync()
    {
        try { return await repository.GetActionTemplatesAsync(); }
        catch (Exception ex) { logger.LogError(ex, "Error loading action templates"); throw; }
    }

    public async Task<List<PrecisionTemplate>> GetPrecisionTemplatesAsync()
    {
        try { return await repository.GetPrecisionTemplatesAsync(); }
        catch (Exception ex) { logger.LogError(ex, "Error loading precision templates"); throw; }
    }

    public async Task<List<AmountTemplate>> GetAmountTemplatesAsync()
    {
        try { return await repository.GetAmountTemplatesAsync(); }
        catch (Exception ex) { logger.LogError(ex, "Error loading amount templates"); throw; }
    }

    public async Task<RuleSet> SaveRuleSetAsync(RuleSet ruleSet)
    {
        try { return await repository.SaveRuleSetAsync(ruleSet); }
        catch (Exception ex) { logger.LogError(ex, "Error saving rule set"); throw; }
    }

    public async Task<Rule> SaveRuleAsync(Rule rule)
    {
        try { return await repository.SaveRuleAsync(rule); }
        catch (Exception ex) { logger.LogError(ex, "Error saving rule"); throw; }
    }
}
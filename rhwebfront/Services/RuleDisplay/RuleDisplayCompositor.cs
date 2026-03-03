using System.Globalization;
using rhdata.Rules;

namespace RHWebFront.Services.RuleDisplay;

public class RuleDisplayCompositor
{
    public RuleComposition ComposeRule(Rule rule)
    {
        ArgumentNullException.ThrowIfNull(rule);

        return new RuleComposition
        {
            TriggerPrompt = ComposeTriggerPrompt(rule),
            PeriodicityPrompt = ComposePeriodicityPrompt(rule),
            ActionPrompt = ComposeActionPrompt(rule),
            AmountPrompt = ComposeAmountPrompt(rule),
            PricePrompt = ComposePricePrompt(rule)
        };
    }

    private static PromptSegment ComposeTriggerPrompt(Rule rule)
    {
        var formatType = rule.Trigger?.TriggerTemplateId == RuleTemplateConstants.TRIGGER_PERCENT_ID ? ValueFormatType.Percentage : ValueFormatType.Currency;
        
        return new PromptSegment
        {
            StaticText = RuleTemplateConstants.TRIGGER_PROMPT_TEXT,
            ConfigFormatType = formatType
        };
    }

    private static PromptSegment ComposePeriodicityPrompt(Rule rule)
    {
        return new PromptSegment
        {
            StaticText = RuleTemplateConstants.PERIODICITY_PROMPT_TEXT,
            ConfigFormatType = ValueFormatType.Number
        };
    }

    private static PromptSegment ComposeActionPrompt(Rule rule)
    {
        return new PromptSegment
        {
            StaticText = RuleTemplateConstants.ACTION_PROMPT_TEXT,
            ShowConfig = false,
            TrailingText = RuleTemplateConstants.ACTION_PROMPT_TRAILING_TEXT
        };
    }

    private static PromptSegment ComposeAmountPrompt(Rule rule)
    {
        var formatType = rule.Amount?.AmountTemplateId switch
        {
            RuleTemplateConstants.AMOUNT_PERCENT_ID => ValueFormatType.Percentage,
            RuleTemplateConstants.AMOUNT_CURRENCY_ID => ValueFormatType.Currency,
            _ => ValueFormatType.Number
        };
        
        return new PromptSegment
        {
            ConfigFormatType = formatType
        };
    }

    private PromptSegment ComposePricePrompt(Rule rule)
    {
        var isMarketAction = rule.Action?.ActionTemplateId is RuleTemplateConstants.ACTION_MARKET_SELL_ID or RuleTemplateConstants.ACTION_MARKET_BUY_ID;
        var amountUnit = DetermineAmountUnit(rule.Amount?.AmountTemplateId);

        if (isMarketAction)
        {
            return new PromptSegment
            {
                StaticText = $"{amountUnit} {RuleTemplateConstants.PRICE_MARKET_SUFFIX}",
                ShowDropdown = false,
                ShowConfig = false
            };
        }

        var priceFormatType = rule.Price?.PriceTemplateId == RuleTemplateConstants.PRICE_PERCENT_ID ? ValueFormatType.Percentage : ValueFormatType.Currency;

        return new PromptSegment
        {
            StaticText = $"{amountUnit} {RuleTemplateConstants.ACTION_PROMPT_TRAILING_TEXT}",
            ConfigFormatType = priceFormatType,
            TrailingText = RuleTemplateConstants.PRICE_LIMIT_TRAILING_TEXT
        };
    }

    private string DetermineAmountUnit(int? amountTemplateId)
    {
        return amountTemplateId switch
        {
            RuleTemplateConstants.AMOUNT_PERCENT_ID  => RuleTemplateConstants.AMOUNT_UNIT_PERCENT,
            RuleTemplateConstants.AMOUNT_CURRENCY_ID => RuleTemplateConstants.AMOUNT_UNIT_CURRENCY,
            _                                        => RuleTemplateConstants.AMOUNT_UNIT_FLAT
        };
    }

    public string FormatValue(decimal value, ValueFormatType formatType)
    {
        return formatType switch
        {
            ValueFormatType.Currency   => value.ToString("C2"),
            ValueFormatType.Percentage => $"{value:0.##}%",
            ValueFormatType.Number     => value.ToString("0.####"),
            _                          => value.ToString(CultureInfo.CurrentCulture)
        };
    }

    public bool ValidateValue(string input, ValueFormatType formatType, out decimal value)
    {
        value = 0;
        if (string.IsNullOrWhiteSpace(input)) return false;

        var cleanInput = input.Trim();
        return formatType switch
        {
            ValueFormatType.Currency   => ValidateCurrency(cleanInput, out value),
            ValueFormatType.Percentage => ValidatePercentage(cleanInput, out value),
            _                          => decimal.TryParse(cleanInput, out value)
        };
    }

    private bool ValidateCurrency(string input, out decimal value)
    {
        value = 0;
        var cleanInput = input.Replace("$", "").Replace(",", "").Trim();
        return decimal.TryParse(cleanInput, out value) && value >= 0;
    }

    private bool ValidatePercentage(string input, out decimal value)
    {
        value = 0;
        var cleanInput = input.Replace("%", "").Trim();
        return decimal.TryParse(cleanInput, out value);
    }
}
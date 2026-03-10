namespace RHWebFront.Services.RuleDisplay;

public static class RuleTemplateConstants
{
    // Template IDs
    public const int TRIGGER_FLAT_ID = 1;
    public const int TRIGGER_PERCENT_ID = 2;
    
    public const int ACTION_LIMIT_SELL_ID = 1;
    public const int ACTION_LIMIT_BUY_ID = 2;
    public const int ACTION_MARKET_SELL_ID = 3;
    public const int ACTION_MARKET_BUY_ID = 4;
    
    public const int PERIODICITY_MINUTES_ID = 1;
    public const int PERIODICITY_HOURS_ID = 2;
    public const int PERIODICITY_DAYS_ID = 3;
    
    public const int AMOUNT_FLAT_ID = 1;
    public const int AMOUNT_PERCENT_ID = 2;
    public const int AMOUNT_CURRENCY_ID = 3;
    
    public const int PRICE_FLAT_ID = 1;
    public const int PRICE_PERCENT_ID = 2;
    
    // Display Text
    public const string TRIGGER_PROMPT_TEXT = "Trigger when market price is";
    public const string PERIODICITY_PROMPT_TEXT = ". Evaluate every";
    public const string ACTION_PROMPT_TEXT = ". Execute a";
    public const string ACTION_PROMPT_TRAILING_TEXT = "at";
    
    public const string AMOUNT_UNIT_FLAT = "assets";
    public const string AMOUNT_UNIT_PERCENT = "of held assets";
    public const string AMOUNT_UNIT_CURRENCY = "worth of held assets";
    
    public const string PRICE_MARKET_SUFFIX = "at market price.";
    public const string PRICE_LIMIT_TRAILING_TEXT = "from market price.";
}
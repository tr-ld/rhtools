namespace RHWebFront.Services.RuleDisplay;

public class RuleComposition
{
    public PromptSegment TriggerPrompt { get; set; }
    public PromptSegment PeriodicityPrompt { get; set; }
    public PromptSegment ActionPrompt { get; set; }
    public PromptSegment AmountPrompt { get; set; }
    public PromptSegment PricePrompt { get; set; }
}
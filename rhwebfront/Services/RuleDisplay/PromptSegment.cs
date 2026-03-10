namespace RHWebFront.Services.RuleDisplay;

public class PromptSegment
{
    public string StaticText { get; set; }
    public bool ShowDropdown { get; set; } = true;
    public bool ShowConfig { get; set; } = true;
    public ValueFormatType ConfigFormatType { get; set; }
    public string TrailingText { get; set; }
}
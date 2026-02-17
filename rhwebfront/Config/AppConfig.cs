namespace RHWebFront.Config;

public class AppConfig
{
    public string TradeCurrency { get; set; } = "USD";
    public int MinimumCadence { get; set; }
    public int DefaultCadence { get; set; }
    public int SelectedCadence { get; set; }
}
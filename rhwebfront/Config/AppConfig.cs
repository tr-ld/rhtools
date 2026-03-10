using System.ComponentModel.DataAnnotations;

namespace RHWebFront.Config;

public class AppConfig
{
    [Required]
    public string TradeCurrency { get; set; } = "USD";
    
    [Range(10, 86400)]
    public int MinimumCadence { get; set; } = 86400;
    
    [Range(10, 86400)]
    public int DefaultCadence { get; set; } = 300;
    
    [Required]
    [Range(10, 86400)]
    public int SelectedCadence { get; set; }
}
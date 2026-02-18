using System.ComponentModel.DataAnnotations;

namespace RHWebFront.Config;

public class CacheConfig
{
    [Required]
    [Range(1, 3600)]
    public int HoldingsCacheSeconds { get; set; }
    
    [Required]
    [Range(1, 300)]
    public int BidAskCacheSeconds { get; set; }
    
    [Required]
    [Range(1, 3600)]
    public int OrdersCacheSeconds { get; set; }
    
    [Required]
    [Range(1, 168)]
    public int TradingPairsCacheHours { get; set; }
}
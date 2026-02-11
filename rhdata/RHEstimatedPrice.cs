using System.ComponentModel.DataAnnotations;

namespace rhdata
{
    public class RHEstimatedPrice : RHBidAsk
    {
        [Required]
        [MaxLength(10)]
        public string Side { get; set; } = default!;   // "bid" or "ask"
    }
}

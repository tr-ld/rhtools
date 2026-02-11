using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHTradingPair
    {
        [Key]
        [MaxLength(20)]
        public string Symbol { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string AssetCode { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string QuoteCode { get; set; } = default!;

        [Column(TypeName = "decimal(38,18)")]
        public decimal QuoteIncrement { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal AssetIncrement { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal MaxOrderSize { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal MinOrderSize { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = default!;
    }

}

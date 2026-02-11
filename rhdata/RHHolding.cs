using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHHolding
    {
        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string AssetCode { get; set; } = default!;

        [Column(TypeName = "decimal(38,18)")]
        public decimal TotalQuantity { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal QuantityAvailableForTrading { get; set; }

        // Navigation
        public RHAccount Account { get; set; } = default!;
    }
}

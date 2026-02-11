using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHOrder
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string Symbol { get; set; } = default!;

        [Required]
        public Guid ClientOrderId { get; set; }

        [Required]
        [MaxLength(10)]
        public string Side { get; set; } = default!;   // "buy" / "sell"

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } = default!;   // "market", etc.

        [Required]
        [MaxLength(20)]
        public string State { get; set; } = default!;  // "filled", etc.

        [Column(TypeName = "decimal(38,18)")]
        public decimal AveragePrice { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal FilledAssetQuantity { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Navigation: Executions
        public ICollection<RHOrderExecution> Executions { get; set; } = new List<RHOrderExecution>();

        // Navigation: Market order config (nullable for non-market orders)
        public RHMarketOrderConfig MarketOrderConfig { get; set; }
    }

}

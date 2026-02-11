using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHBidAsk
    {
        [Key]
        public int Id { get; set; }   // surrogate key

        [Required]
        [MaxLength(20)]
        public string Symbol { get; set; } = default!; // e.g., "ETH-USD"

        [Column(TypeName = "decimal(38,18)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal BidInclusiveOfSellSpread { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal SellSpread { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal AskInclusiveOfBuySpread { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal BuySpread { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}

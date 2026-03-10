using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata;

public class BidAskHistoryEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = default!;

    [Column(TypeName = "decimal(38,18)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(38,18)")]
    public decimal SellSpread { get; set; }

    [Column(TypeName = "decimal(38,18)")]
    public decimal BuySpread { get; set; }

    public DateTimeOffset Timestamp { get; set; }
}
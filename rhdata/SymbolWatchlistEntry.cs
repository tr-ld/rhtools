using System.ComponentModel.DataAnnotations;

namespace rhdata;

public class SymbolWatchlistEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
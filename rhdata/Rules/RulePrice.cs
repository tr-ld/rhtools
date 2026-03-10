using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RulePrice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PriceTemplateId { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal Value { get; set; }

        [ForeignKey(nameof(PriceTemplateId))]
        public PriceTemplate PriceTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
    
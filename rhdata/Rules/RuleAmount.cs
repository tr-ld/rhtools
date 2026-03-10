using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RuleAmount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AmountTemplateId { get; set; }

        [Required]
        [Column(TypeName = "decimal(38,18)")]
        public decimal Value { get; set; }

        [ForeignKey(nameof(AmountTemplateId))]
        public AmountTemplate AmountTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
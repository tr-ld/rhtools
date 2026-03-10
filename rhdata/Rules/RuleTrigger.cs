using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RuleTrigger
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TriggerTemplateId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }

        [ForeignKey(nameof(TriggerTemplateId))]
        public TriggerTemplate TriggerTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RuleAction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ActionTemplateId { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal Value { get; set; }

        [ForeignKey(nameof(ActionTemplateId))]
        public ActionTemplate ActionTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RulePrecision
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PrecisionTemplateId { get; set; }

        [Required]
        public int Value { get; set; }

        [ForeignKey(nameof(PrecisionTemplateId))]
        public PrecisionTemplate PrecisionTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
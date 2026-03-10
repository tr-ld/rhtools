using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class RulePeriodicity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PeriodicityTemplateId { get; set; }

        [Required]
        public int Value { get; set; }

        [ForeignKey(nameof(PeriodicityTemplateId))]
        public PeriodicityTemplate PeriodicityTemplate { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
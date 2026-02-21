using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class Rule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RuleSetId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int TriggerId { get; set; }

        [Required]
        public int ActionId { get; set; }

        [Required]
        public int PrecisionId { get; set; }

        [Required]
        public int AmountId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        [ForeignKey(nameof(RuleSetId))]
        public RuleSet RuleSet { get; set; } = default!;

        [ForeignKey(nameof(PositionId))]
        public RuleOrderPosition Position { get; set; } = default!;

        [ForeignKey(nameof(TriggerId))]
        public RuleTrigger Trigger { get; set; } = default!;

        [ForeignKey(nameof(ActionId))]
        public RuleAction Action { get; set; } = default!;

        [ForeignKey(nameof(PrecisionId))]
        public RulePrecision Precision { get; set; } = default!;

        [ForeignKey(nameof(AmountId))]
        public RuleAmount Amount { get; set; } = default!;
    }
}
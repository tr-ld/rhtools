using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata.Rules
{
    public class Rule : IEquatable<Rule>, ICloneable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RuleSetId { get; set; }

        [Required]
        public int Position { get; set; }

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

        [ForeignKey(nameof(TriggerId))]
        public RuleTrigger Trigger { get; set; } = default!;

        [ForeignKey(nameof(ActionId))]
        public RuleAction Action { get; set; } = default!;

        [ForeignKey(nameof(PrecisionId))]
        public RulePrecision Precision { get; set; } = default!;

        [ForeignKey(nameof(AmountId))]
        public RuleAmount Amount { get; set; } = default!;

        public bool Equals(Rule other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return IsActive == other.IsActive &&
                   Trigger?.TriggerTemplateId == other.Trigger?.TriggerTemplateId &&
                   Trigger?.Value == other.Trigger?.Value &&
                   Action?.ActionTemplateId == other.Action?.ActionTemplateId &&
                   Action?.Value == other.Action?.Value &&
                   Precision?.PrecisionTemplateId == other.Precision?.PrecisionTemplateId &&
                   Precision?.Value == other.Precision?.Value &&
                   Amount?.AmountTemplateId == other.Amount?.AmountTemplateId &&
                   Amount?.Value == other.Amount?.Value;
        }

        public override bool Equals(object obj) => Equals(obj as Rule);

        public override int GetHashCode() => HashCode.Combine(IsActive, Trigger?.TriggerTemplateId, Trigger?.Value, Action?.ActionTemplateId, Action?.Value, Precision?.PrecisionTemplateId, Precision?.Value, Amount?.AmountTemplateId);

        public object Clone()
        {
            return new Rule
            {
                Id = Id,
                RuleSetId = RuleSetId,
                Position = Position,
                TriggerId = TriggerId,
                ActionId = ActionId,
                PrecisionId = PrecisionId,
                AmountId = AmountId,
                IsActive = IsActive,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                Trigger = new RuleTrigger 
                { 
                    Id = Trigger?.Id ?? 0,
                    TriggerTemplateId = Trigger?.TriggerTemplateId ?? 0,
                    TriggerTemplate = Trigger?.TriggerTemplate,
                    Value = Trigger?.Value ?? 0 
                },
                Action = new RuleAction 
                { 
                    Id = Action?.Id ?? 0,
                    ActionTemplateId = Action?.ActionTemplateId ?? 0,
                    ActionTemplate = Action?.ActionTemplate,
                    Value = Action?.Value ?? 0 
                },
                Precision = new RulePrecision 
                { 
                    Id = Precision?.Id ?? 0,
                    PrecisionTemplateId = Precision?.PrecisionTemplateId ?? 0,
                    PrecisionTemplate = Precision?.PrecisionTemplate,
                    Value = Precision?.Value ?? 0 
                },
                Amount = new RuleAmount 
                { 
                    Id = Amount?.Id ?? 0,
                    AmountTemplateId = Amount?.AmountTemplateId ?? 0,
                    AmountTemplate = Amount?.AmountTemplate,
                    Value = Amount?.Value ?? 0 
                }
            };
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace rhdata.Rules
{
    public class RuleSet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Symbol { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public List<Rule> Rules { get; set; } = [];
    }
}
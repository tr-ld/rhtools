using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public List<Rule> Rules { get; set; } = [];
    }
}
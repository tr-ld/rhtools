using System.ComponentModel.DataAnnotations;

namespace rhdata.Rules
{
    public class RuleOrderPosition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Position { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
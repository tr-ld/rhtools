using System.ComponentModel.DataAnnotations;

namespace rhdata.Rules
{
    public class PeriodicityTemplate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = default!;

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = default!;

        public DateTimeOffset CreatedAt { get; set; }
        
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
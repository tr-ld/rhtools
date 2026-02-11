using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHOrderExecution
    {
        [Key]
        public int Id { get; set; }   // surrogate key

        [Column(TypeName = "decimal(38,18)")]
        public decimal EffectivePrice { get; set; }

        [Column(TypeName = "decimal(38,18)")]
        public decimal Quantity { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        // Foreign key
        public Guid OrderId { get; set; }

        public RHOrder Order { get; set; } = default!;
    }

}

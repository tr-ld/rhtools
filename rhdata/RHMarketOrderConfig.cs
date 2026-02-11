using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHMarketOrderConfig
    {
        [Key]
        public int Id { get; set; }   // surrogate key

        [Column(TypeName = "decimal(38,18)")]
        public decimal AssetQuantity { get; set; }

        // Foreign key
        public Guid OrderId { get; set; }

        public RHOrder Order { get; set; } = default!;
    }

}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace rhdata
{
    public class RHAccount
    {
        [Key]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = default!;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = default!;

        [Column(TypeName = "decimal(18,4)")]
        public decimal BuyingPower { get; set; }

        [Required]
        [MaxLength(3)]
        public string BuyingPowerCurrency { get; set; } = default!;
    }

}

using LMS.Data_.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class Plan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 120)]
        public int DurationInMonth { get; set; }



        public CurrencyEnum Currency { get; set; } = CurrencyEnum.EGP;

        public ICollection<Subscriptions>? Subscriptions { get; set; } = new List<Subscriptions>();
    }
}

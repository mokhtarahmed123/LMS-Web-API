using LMS.Data_.Enum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    [Index(nameof(TransactionId), IsUnique = true)]
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1_000_000)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }
        public CurrencyEnum Currency { get; set; }
        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; } = PaymentMethodEnum.Wallet;

        [Required]
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString("N");

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public DateTime? UpdatedAt { get; set; }


        [Required]
        public int SubscriptionId { get; set; }

        [ForeignKey(nameof(SubscriptionId))]
        public Subscriptions Subscription { get; set; } = null!;
        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public Users user { get; set; } = null!;


    }
}

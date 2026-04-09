using LMS.Data_.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class Subscriptions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; } = false;


        public SubscriptionStatusEnum Status { get; set; } = SubscriptionStatusEnum.Pending;



        public int Discount { get; set; } = 0;
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public Users User { get; set; } = null!;





        [Required]
        public int PlanId { get; set; }
        [ForeignKey(nameof(PlanId))]
        public Plan Plan { get; set; }

    }
}

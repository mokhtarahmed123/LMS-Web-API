using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Data_.Entities
{
    public class UserCoupons
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        [Key, Column(Order = 1)]
        public int CouponId { get; set; }



        public DateTime? UsedAt { get; set; } = null;

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [ForeignKey("CouponId")]
        public virtual Coupons Coupon { get; set; }



    }
}

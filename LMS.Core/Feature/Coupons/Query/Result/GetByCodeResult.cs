using LMS.Data_.Enum;

namespace LMS.Core.Feature.Coupons.Query.Result
{
    public class GetByCodeResult
    {
        public int Id { get; set; }


        public string Code { get; set; }


        public DiscountTypeEnum DiscountType { get; set; }


        public decimal DiscountValue { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }


        public int UsageLimit { get; set; }


        public int UsedCount { get; set; } = 0;

        public bool IsActive { get; set; } = true;


        public DateTime CreatedAt { get; set; }

    }
}

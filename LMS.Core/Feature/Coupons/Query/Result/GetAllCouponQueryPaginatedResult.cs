using LMS.Data_.Enum;

namespace LMS.Core.Feature.Coupons.Query.Result
{
    public class GetAllCouponQueryPaginatedResult
    {
        public int Id { get; set; }


        public string Code { get; set; }
        public DiscountTypeEnum DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public GetAllCouponQueryPaginatedResult(int Id, string Code, DiscountTypeEnum DiscountType, decimal DiscountValue, DateOnly StartDate, DateOnly EndDate, int UsageLimit, int UsedCount, bool IsActive, DateTime CreatedAt)
        {
            this.Id = Id;
            this.Code = Code;
            this.DiscountType = DiscountType;
            this.DiscountValue = DiscountValue;
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            this.UsageLimit = UsageLimit;
            this.UsedCount = UsedCount;
            this.IsActive = IsActive;
            this.CreatedAt = CreatedAt;


        }

    }
}

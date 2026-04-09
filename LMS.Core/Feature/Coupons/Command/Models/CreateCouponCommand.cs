using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Coupons.Command.Models
{
    public class CreateCouponCommand : IRequest<Response<string>>
    {
        public int CountOfCoupons { get; set; }

        [JsonIgnore]
        public string Code { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

        public DiscountTypeEnum DiscountType { get; set; } = DiscountTypeEnum.Percentage;
        public decimal DiscountValue { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; } = true;
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



    }
}

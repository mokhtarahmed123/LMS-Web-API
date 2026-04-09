using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Coupons.Command.Models
{
    public class UpdateCouponCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public DiscountTypeEnum DiscountType { get; set; } = DiscountTypeEnum.Percentage;
        public decimal DiscountValue { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }

        public UpdateCouponCommand(int Id)
        {
            this.Id = Id;
        }

    }
}

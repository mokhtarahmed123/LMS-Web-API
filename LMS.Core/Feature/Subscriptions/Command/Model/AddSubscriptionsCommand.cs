using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Subscriptions.Command.Model
{
    public class AddSubscriptionsCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public DateOnly StartDate { get; set; }
        [JsonIgnore]
        public DateOnly EndDate { get; set; }

        public string? CouponCode { get; set; } = null;

        [JsonIgnore]
        public bool IsActive { get; set; } = false;
        //public string UserId { get; set; } //   => We Will Get It From JWT
        public int PlanId { get; set; }
    }
}

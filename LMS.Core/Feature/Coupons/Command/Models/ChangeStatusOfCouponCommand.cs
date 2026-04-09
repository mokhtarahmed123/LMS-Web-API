using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Coupons.Command.Models
{
    public class ChangeStatusOfCouponCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public bool IsActive { get; set; }

    }
}

using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Subscriptions.Command.Model
{
    public class RenewSubscriptionsCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int SubscriptionId { get; set; }
        public int PlanId { get; set; }
        public PaymentMethodEnum paymentMethod { get; set; }



    }
}

using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Subscriptions.Command.Model
{
    public class ChangeSubscriptionPlanCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int SubscriptionId { get; set; }
        [JsonIgnore]
        public DateOnly EndDate { get; set; }

        public int PlanId { get; set; }
        public ChangeSubscriptionPlanCommand(int Id)
        {
            this.SubscriptionId = Id;
        }
        public ChangeSubscriptionPlanCommand()
        {

        }
    }
}

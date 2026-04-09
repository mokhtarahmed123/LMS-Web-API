using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Plan.Command.Models
{
    public class UpdatePlanCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public CurrencyEnum Currency { get; set; }
        public UpdatePlanCommand()
        {

        }
    }
}

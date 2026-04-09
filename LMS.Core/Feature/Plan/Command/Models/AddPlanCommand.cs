using LMS.Data_.Enum;
using MediatR;
namespace LMS.Core.Feature.Plan.Command.Models
{
    public class AddPlanCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }
        public CurrencyEnum Currency { get; set; }

    }
}

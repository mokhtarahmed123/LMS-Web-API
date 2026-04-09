using LMS.Core.Feature.Plan.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.PlanMapping
{
    public partial class PlanProfile
    {
        public void GetById()
        {
            CreateMap<Plan, GetPlanByIdResult>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
            .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price))
             .ForMember(x => x.Currency, opt => opt.MapFrom(x => x.Currency.ToString()))
            .ForMember(x => x.DurationInMonth, opt => opt.MapFrom(x => x.DurationInMonth));


        }
    }
}

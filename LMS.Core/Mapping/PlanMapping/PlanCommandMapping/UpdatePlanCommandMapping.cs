using LMS.Core.Feature.Plan.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.PlanMapping
{
    public partial class PlanProfile
    {
        public void Update()
        {
            CreateMap<UpdatePlanCommand, Plan>()
             .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
             .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price))
             .ForMember(x => x.Currency, opt => opt.MapFrom(x => x.Currency))
             .ForMember(x => x.DurationInMonth, opt => opt.MapFrom(x => x.DurationInMonths));


        }
    }
}

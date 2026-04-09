using AutoMapper;

namespace LMS.Core.Mapping.PlanMapping
{
    public partial class PlanProfile : Profile
    {
        public PlanProfile()
        {
            Add();
            Update();
            GetAllPlansForUser();
            GetById();
        }
    }
}

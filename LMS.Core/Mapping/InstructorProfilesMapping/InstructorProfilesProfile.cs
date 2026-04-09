using AutoMapper;

namespace LMS.Core.Mapping.InstructorProfilesMapping
{
    public partial class InstructorProfilesProfile : Profile
    {
        public InstructorProfilesProfile()
        {
            Add();
            GetALL();
            update();
            GetById();
            GetInstructorByUserIdProfileMapping();
            GetAllInstructorProfilesByFilterQueryMapping();
            GetMyRequest();
        }
    }
}

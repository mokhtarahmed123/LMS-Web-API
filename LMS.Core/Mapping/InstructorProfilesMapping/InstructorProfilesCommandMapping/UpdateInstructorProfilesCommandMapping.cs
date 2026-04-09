using LMS.Core.Feature.InstructorProfiles.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.InstructorProfilesMapping
{
    public partial class InstructorProfilesProfile
    {
        public void update()
        {
            CreateMap<UpdateInstructorProfileCommand, InstructorProfiles>()
              .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
              .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl))
               .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio));



        }
    }
}

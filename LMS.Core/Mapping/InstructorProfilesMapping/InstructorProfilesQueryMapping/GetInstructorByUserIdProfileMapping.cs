using LMS.Core.Feature.InstructorProfiles.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.InstructorProfilesMapping
{
    public partial class InstructorProfilesProfile
    {

        public void GetInstructorByUserIdProfileMapping()
        {
            CreateMap<InstructorProfiles, GetInstructorByUserIdProfileResult>
                ()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                     .ForMember(dest => dest.ReasonOfRejected, opt => opt.MapFrom(src => src.ReasonOfReject))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
                .ForMember(dest => dest.StatusOfInstructor, opt => opt.MapFrom(src => src.StatusOfInstructorProfile.ToString()))

                .ForMember(dest => dest.LinkedInUrl, opt => opt.MapFrom(src => src.LinkedInUrl));


        }
    }
}

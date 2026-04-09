using LMS.Core.Feature.InstructorProfiles.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.InstructorProfilesMapping
{
    public partial class InstructorProfilesProfile
    {
        public void GetAllInstructorProfilesByFilterQueryMapping()
        {
            CreateMap<InstructorProfiles, GetAllInstructorProfilesByFilterResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ReasonOfReject, opt => opt.MapFrom(src => src.ReasonOfReject))
                .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
                .ForMember(dest => dest.LinkedinUrl, opt => opt.MapFrom(src => src.LinkedInUrl))

                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.CreatedAt)))
                .ForMember(dest => dest.StatusOfInstructorProfile, opt => opt.MapFrom(src => src.StatusOfInstructorProfile.ToString()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}

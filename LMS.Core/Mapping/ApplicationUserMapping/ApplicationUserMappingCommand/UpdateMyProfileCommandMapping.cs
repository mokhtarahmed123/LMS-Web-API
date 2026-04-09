using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void Update()
        {
            CreateMap<UpdateMyProfileCommand, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));


        }
    }
}

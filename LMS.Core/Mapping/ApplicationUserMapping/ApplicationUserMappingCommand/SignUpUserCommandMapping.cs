using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void AddUserCommandMapping()
        {
            CreateMap<SignUpUserCommand, Users>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}

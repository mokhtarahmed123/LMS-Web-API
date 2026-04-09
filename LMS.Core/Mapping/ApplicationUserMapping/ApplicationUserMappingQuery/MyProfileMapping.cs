using LMS.Core.Feature.ApplicationUser.Query.Result.ProfileResult;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void MyProfile()
        {
            CreateMap<Users, MyProfileResult>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email));

        }
    }
}

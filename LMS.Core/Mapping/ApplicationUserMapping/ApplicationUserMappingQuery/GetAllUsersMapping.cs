using LMS.Core.Feature.ApplicationUser.Query.Result.AdminResult;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void GetAllUsersMapping()
        {
            CreateMap<Users, GetUsersQueryResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email));
        }
    }
}

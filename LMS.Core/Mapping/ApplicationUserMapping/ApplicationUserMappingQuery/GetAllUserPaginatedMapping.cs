using LMS.Core.Feature.ApplicationUser.Query.Result.AdminResult;
using LMS.Data_.Helper;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void GetAllUserPaginatedMapping()
        {
            CreateMap<UserWithRoleDto, GetAllUserPaginatedResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}

using LMS.Core.Feature.Authorization.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile
    {
        public void AddGetAllUsersMapping()
        {
            CreateMap<Users, GetAllUsersQueryResultWithRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email))
                ;
        }
    }
}

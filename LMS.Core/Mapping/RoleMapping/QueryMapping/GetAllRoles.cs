using LMS.Core.Feature.Authorization.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.AuthorizationMapping
{
    public partial class AuthorizationProfile
    {
        public void GetAllRolesMapping()
        {

            CreateMap<Role, GetAllRolesResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}

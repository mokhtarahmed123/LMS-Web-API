using AutoMapper;

namespace LMS.Core.Mapping.AuthorizationMapping
{
    public partial class AuthorizationProfile : Profile
    {
        public AuthorizationProfile()
        {
            GetAllRolesMapping();
        }

    }
}

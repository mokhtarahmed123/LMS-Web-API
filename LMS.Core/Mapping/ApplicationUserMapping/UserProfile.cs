using AutoMapper;

namespace LMS.Core.Mapping.ApplicationUserMapping
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {

            AddUserCommandMapping();
            AddGetAllUsersMapping();
            GetAllUsersMapping();
            GetUserByIdMapping();
            Update();
            MyProfile();
        }
    }
}

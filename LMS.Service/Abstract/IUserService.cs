using LMS.Data_.Entities;
using LMS.Data_.Helper;

namespace LMS.Service.Abstract
{
    public interface IUserService
    {
        public Task<List<Users>> GetAll();
        public Task<bool> EmailNotRepeatedWhenUpdated(string Id, string Email);
        public IQueryable<UserWithRoleDto> GetAllUsers();

    }
}

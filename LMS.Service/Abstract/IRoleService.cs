using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRoles();

        Task<Role> GetRoleById(string Id);
        Task<Role> GetRoleByName(string name);

    }
}

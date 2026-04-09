using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface IRoleRepository : IGenericRepositoryAsync<Role>
    {
        Task<List<Role>> GetAllRoles();
    }
}

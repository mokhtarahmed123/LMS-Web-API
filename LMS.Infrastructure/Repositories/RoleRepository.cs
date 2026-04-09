using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepositoryAsync<Role>, IRoleRepository
    {
        private readonly DbSet<Role> Role;
        public RoleRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            Role = appDbContext.Set<Role>();

        }

        //public async Task<List<IdentityRole>> GetAllRoles()
        //{

        //    AppDbContext appDbContext = new AppDbContext();

        //    return await appDbContext.Roles.Include(a => a.uuserds).ToListAsync();
        //}

        public Task<List<Role>> GetAllRoles()
        {
            return Role.Include(a => a.UserRoles).ToListAsync();
        }
    }
}

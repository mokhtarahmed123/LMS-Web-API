using LMS.Data_.Entities;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> roleManager;

        public RoleService(RoleManager<Role> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await roleManager.Roles.Include(a => a.UserRoles).ToListAsync();
        }


        public async Task<Role> GetRoleByName(string name)
        {
            return await roleManager.Roles.Include(a => a.UserRoles).FirstOrDefaultAsync(n => n.Name.ToLower() == name.ToLower());
        }

        public async Task<Role> GetRoleById(string id)
        {
            return await roleManager.Roles.Include(a => a.UserRoles).FirstOrDefaultAsync(n => n.Id == id);
        }
    }
}

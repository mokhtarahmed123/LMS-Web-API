using LMS.Data_.Entities;
using LMS.Data_.Helper;
using LMS.Infrastructure.Context;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<Users> userManager;
        private readonly AppDbContext appDbContext;

        public UserService(UserManager<Users> userManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public async Task<bool> EmailNotRepeatedWhenUpdated(string Id, string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);

            if (user == null)
                return true;


            if (user.Id != Id)
                return false;


            return true;
        }

        public async Task<List<Users>> GetAll()
        {
            return await userManager.Users.ToListAsync();
        }



        public IQueryable<UserWithRoleDto> GetAllUsers()
        {
            return from user in appDbContext.Users
                   join userRole in appDbContext.UserRoles on user.Id equals userRole.UserId into userRoles
                   from ur in userRoles.DefaultIfEmpty()
                   join role in appDbContext.Roles on ur.RoleId equals role.Id into roles
                   from r in roles.DefaultIfEmpty()
                   select new UserWithRoleDto
                   {
                       Id = user.Id,
                       UserName = user.UserName,
                       Email = user.Email,
                       IsActive = user.EmailConfirmed,
                       RoleName = r.Name
                   };
        }
    }
}

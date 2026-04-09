using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Authorization.Query.Models;
using LMS.Core.Feature.Authorization.Query.Result;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Data;

namespace LMS.Core.Feature.Authorization.Query.Handler
{
    public class RolesQueryHandlers : ResponseHandler,
        IRequestHandler<GetAllRolesQuery, Response<List<GetAllRolesResult>>>
        , IRequestHandler<GetUsersByRoleNameQuery, Response<GetUsersByRoleNameResult>>
    {
        private readonly IRoleService authorizationService;
        private readonly RoleManager<Role> roleManager;
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public RolesQueryHandlers(IRoleService authorizationService, RoleManager<Role> roleManager, IMapper mapper, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.authorizationService = authorizationService;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
        }


        public async Task<Response<List<GetAllRolesResult>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var response = new List<GetAllRolesResult>();
            var roles = await authorizationService.GetAllRoles();

            foreach (var role in roles)
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);

                var usersMapped = usersInRole.Select(u => new GetAllUsersQueryResultWithRole
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    UserEmail = u.Email
                }).ToList();

                response.Add(new GetAllRolesResult
                {
                    Id = role.Id,
                    Name = role.Name,
                    UsersCount = usersMapped.Count,
                    Users = usersMapped
                });
            }

            return Success(response);
        }

        public async Task<Response<GetUsersByRoleNameResult>> Handle(GetUsersByRoleNameQuery request, CancellationToken cancellationToken)
        {
            var response = new GetUsersByRoleNameResult();
            var role = await authorizationService.GetRoleByName(request.RoleName);
            if (role == null)
            {
                return NotFound<GetUsersByRoleNameResult>();
            }
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Count <= 0)
            {
                return NotFound<GetUsersByRoleNameResult>();
            }
            var usersMapped = usersInRole.Select(u => new GetAllUsersQueryResultWithRole
            {
                Id = u.Id,
                UserName = u.UserName,
                UserEmail = u.Email
            }).ToList();

            response.CountOfUsers = usersMapped.Count;
            response.Users = usersMapped;
            response.RoleName = role.Name;
            return Success(response);
        }
    }
}

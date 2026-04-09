using LMS.API.Bases;
using LMS.Core.Feature.Authorization.Command.Models;
using LMS.Core.Feature.Authorization.Query.Models;
using LMS.Core.Feature.Authorization.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : AppBaseController
    {


        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_Roles_user_{UserId}";
        private string GetUsersByRoleNamecacheKey => $"all_GetUsersByRoleNameCourse_user_{UserId}";


        #region Post

        [HttpPost("roles")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [SwaggerOperation(
            Summary = "Create role",
            Description = "Create a new role in the system"
        )]
        public async Task<IActionResult> AddRole(AddRoleCommand name)
        {
            var response = await Mediator.Send(name);
            if (response.Succeeded)
            {
                await ClearCache();
            }

            return NewResult(response);
        }


        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPost("users/{email}/roles/reset")]
        [SwaggerOperation(Summary = "Reset user role", Description = "Remove all roles from the user and reset them")]
        public async Task<IActionResult> ResetUserRole([FromRoute] string email)
        {
            var response = await Mediator.Send(new ResetUserRoleCommand(email));
            if (response.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(response);
        }

        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPost("roles/users")]
        [SwaggerOperation(
          Summary = "Assign role to user",
          Description = "Assign a specific role to a user by email"
      )]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] AssignRoleToUserCommand command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(response);
        }

        #endregion

        #region Get

        [HttpGet("roles")]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(Summary = "Get all roles", Description = "Retrieve all system roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var Role = await RedisCacheService.GetDataAsync<List<GetAllRolesResult>>(cacheKey);
            if (Role != null)
            {
                return Ok(Role);
            }
            var response = await Mediator.Send(new GetAllRolesQuery());
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(20));
            }

            return NewResult(response);
        }


        [HttpGet("roles/{Name}/users")]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(Summary = "Get users by role", Description = "Retrieve all users assigned to a specific role")]
        public async Task<IActionResult> GetUsersByRoleName([FromRoute] string Name)
        {
            var Role = await RedisCacheService.GetDataAsync<GetUsersByRoleNameResult>(GetUsersByRoleNamecacheKey);
            if (Role != null)
            {
                return Ok(Role);
            }

            var response = await Mediator.Send(new GetUsersByRoleNameQuery(Name));
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(GetUsersByRoleNamecacheKey, response.Data, TimeSpan.FromMinutes(20));
            }

            return NewResult(response);
        }
        #endregion


        #region Put && Patch
        [HttpPut("roles")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [SwaggerOperation(Summary = "Update role", Description = "Update role name by role id")]
        public async Task<IActionResult> UpdateRole(

    [FromBody] UpdateRoleCommand command,
    CancellationToken cancellationToken)
        {
            var request = new UpdateRoleCommand(command.Id, command.Name);

            var response = await Mediator.Send(request, cancellationToken);
            if (response.Succeeded)
            {
                await ClearCache();
            }

            return NewResult(response);
        }
        #endregion


        #region Delete

        [HttpDelete("roles/{id}")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [SwaggerOperation(
            Summary = "Delete role",
            Description = "Delete role by role id"
        )]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {
            var response = await Mediator.Send(new DeleteRoleCommand(id));
            if (response.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(response);
        }


        #endregion

        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(GetUsersByRoleNamecacheKey);
        }


    }
}
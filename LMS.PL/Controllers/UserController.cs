using LMS.API.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.AdminModel;
using LMS.Core.Feature.ApplicationUser.Query.Models.AdminModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : AppBaseController
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await Mediator.Send(new GetAllUsersQuery());
            return NewResult(response);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUser([FromRoute] string Id)
        {
            var response = await Mediator.Send(new GetUserByIdQuery(Id));
            return NewResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var response = await Mediator.Send(new DeleteUserCommand(id));
            return NewResult(response);
        }

        //[HttpGet("GetAllPaginated")]
        //public async Task<IActionResult> GetAllPaginated([FromQuery] GetAllUserPaginatedQuery query)
        //{
        //    var response = await Mediator.Send(query);
        //    return Ok(response);
        //}
    }
}

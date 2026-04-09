using LMS.API.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.ProfileModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : AppBaseController
    {

        [HttpGet("My")]
        public async Task<IActionResult> My()
        {
            var Response = await Mediator.Send(new MyProfileQuery());
            return NewResult(Response);

        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateMyProfileCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var response = await Mediator.Send(new DeleteMyProfileCommand());
            return NewResult(response);

        }
    }
}

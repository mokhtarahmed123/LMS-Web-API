using LMS.API.Bases;
using LMS.Core.Feature.ApplicationUser.Command.Models.AuthModel;
using LMS.Core.Feature.ApplicationUser.Query.Models.AuthModel;
using LMS.Core.Feature.Emails.Command.Models;
using LMS.Core.Feature.Emails.Query.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AppBaseController
    {
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromQuery] SignUpUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPost("LogIn")]
        public async Task<IActionResult> Login([FromQuery] LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);

        }

        [Authorize]
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await Mediator.Send(new LogOutCommand());
            return NewResult(response);

        }


        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpGet("ConfirmEmail")]

        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPost("SendResetPassword")]
        public async Task<IActionResult> SendResetPassword([FromQuery] SendResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);

        }
        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpGet("ConfirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordQuery command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);

        }
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);


        }
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);


        }

    }
}

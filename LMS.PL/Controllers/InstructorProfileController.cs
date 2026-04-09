using LMS.API.Bases;
using LMS.Core.Feature.Authorization.Command.Models;
using LMS.Core.Feature.InstructorProfiles.Command.Models;
using LMS.Core.Feature.InstructorProfiles.Query.Models;
using LMS.Core.Feature.InstructorProfiles.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorProfileController : AppBaseController
    {
        private readonly IWebHostEnvironment _env;
        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_InstructorProfile_user_{UserId}";

        public InstructorProfileController(IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
        }


        #region Post
        [Authorize]
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPost]
        [SwaggerOperation(
    Summary = "Create instructor profile request",
    Description = "Submit a request to become an instructor with profile information and files")]
        public async Task<IActionResult> AddInstructorProfile([FromForm] AddInstructorProfileCommand command)
        {

            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                // Invalidate the cache for all coupons when a new coupon is created
                await ClearCache();
            }


            return NewResult(response);
        }
        #endregion


        #region Get
        [EnableRateLimiting("SlidingWindowPolicy")]

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all instructor profiles",
            Description = "Retrieve all instructor profile requests"
        )]
        public async Task<IActionResult> GetAllInstructorProfiles()
        {
            var InstructorProfile = await RedisCacheService.GetDataAsync<List<GetAllInstructorProfilesResult>>(cacheKey);
            if (InstructorProfile != null)
            {
                return Ok(InstructorProfile);
            }

            var response = await Mediator.Send(new GetAllInstructorProfilesQuery());
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }



        [EnableRateLimiting("SlidingWindowPolicy")]

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get instructor profile by id",
            Description = "Retrieve instructor profile request by id"
        )]
        public async Task<IActionResult> GetByIdInstructorProfile(int id)
        {
            var response = await Mediator.Send(new GetByIdInstructorProfileQuery(id));
            return NewResult(response);
        }

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("SlidingWindowPolicy")]
        [SwaggerOperation(
            Summary = "Get instructor profile by user id",
            Description = "Retrieve instructor profile associated with a specific user"
        )]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetInstructorByUserIdProfile(string userId)
        {
            var response = await Mediator.Send(new GetInstructorByUserIdProfileQuery(userId));
            return NewResult(response);
        }

        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpGet("Paginated")]
        [SwaggerOperation(
            Summary = "Get paginated instructor profiles",
            Description = "Retrieve instructor profile requests in a paginated format"
        )]
        public async Task<IActionResult> GetPaginatedInstructorProfiles([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var response = await Mediator.Send(new GetAlInstructorProfilesPaginatedQuery(pageNumber, pageSize));
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Filter")]
        [EnableRateLimiting("TokenBucketPolicy")]
        public async Task<IActionResult> GetAllInstructorProfilesByFilter([FromQuery] GetAllInstructorProfilesByFilterQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [Authorize]
        [HttpGet("MyRequest")]
        [EnableRateLimiting("TokenBucketPolicy")]
        public async Task<IActionResult> GetMyRequest()
        {

            var response = await Mediator.Send(new GetMyRequestInstructorProfileQuery());
            return NewResult(response);
        }

        [Authorize(Roles = "Instructor")]
        [HttpGet("MyStats")]
        [EnableRateLimiting("TokenBucketPolicy")]

        [SwaggerOperation(
     Summary = "My Stats    ",
     Description = "My Stats")]
        public async Task<IActionResult> MyStats()
        {
            var Result = await Mediator.Send(new MyStatsAsInstructorQuery());
            return NewResult(Result);
        }


        #endregion


        #region Put && Patch



        [EnableRateLimiting("FixedWindowPolicy")]

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/approve")]
        [SwaggerOperation(Summary = "Approve instructor request", Description = "Approve instructor role request by admin")]
        public async Task<IActionResult> ApproveInstructor(int id)
        {
            var result = await Mediator.Send(
                new AcceptInstructorRoleRequestCommand(id));
            if (result.Succeeded)
            {

                await ClearCache();
            }

            return NewResult(result);

        }

        [EnableRateLimiting("FixedWindowPolicy")]

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/reject")]
        [SwaggerOperation(
            Summary = "Reject instructor request",
            Description = "Reject instructor role request by admin"
        )]
        public async Task<IActionResult> RejectInstructor(int id, [FromQuery] string Reason)
        {
            var result = await Mediator.Send(
                new RejectInstructorRoleRequestCommand(id, Reason));

            if (result.Succeeded)
            {

                await ClearCache();
            }

            return NewResult(result);
        }


        [EnableRateLimiting("TokenBucketPolicy")]
        [Authorize]
        [HttpPut("MyRequest")]
        [SwaggerOperation(
            Summary = "Update instructor profile",
            Description = "Update instructor profile information"
        )]
        public async Task<IActionResult> UpdateMyRequestAsInstructorProfile(
            [FromForm] UpdateInstructorProfileCommand command)
        {
            var response = await Mediator.Send(command);
            if (response != null && response.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(response);
        }
        #endregion


        #region Delete

        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize]
        [HttpDelete("cancel")]
        [SwaggerOperation(
            Summary = "Cancel instructor request",
            Description = "User cancels their instructor profile request"
        )]
        public async Task<IActionResult> CancelRequest()
        {
            var response = await Mediator.Send(new CancelRequestInstructorProfileCommand());
            if (response != null && response.Succeeded)
            {
                await ClearCache();
            }



            return NewResult(response);
        }

        #endregion


        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
        }

    }
}
using LMS.API.Bases;
using LMS.Core.Feature.Plan.Command.Models;
using LMS.Core.Feature.Plan.Query.Models;
using LMS.Core.Feature.Plan.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : AppBaseController
    {
        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_Plan_user_{UserId}";


        #region Post
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPost]
        [SwaggerOperation(Summary = "Create new plan", Description = "Add a new subscription plan")]
        public async Task<IActionResult> Add(AddPlanCommand addPlanCommand)
        {
            var Result = await Mediator.Send(addPlanCommand);
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }
        #endregion

        #region Put && Patch

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPut("{Id}")]
        [SwaggerOperation(
            Summary = "Update plan",
            Description = "Update existing subscription plan by id"
        )]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdatePlanCommand command)
        {
            command.Id = Id;
            var Result = await Mediator.Send(command);
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }

        #endregion

        #region Get
        [AllowAnonymous]
        [HttpGet]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(
           Summary = "Get all plans",
           Description = "Retrieve all available subscription plans for users"
       )]
        public async Task<IActionResult> GetAll()
        {
            var Plan = await RedisCacheService.GetDataAsync<List<GetAllPlanResultForUsers>>(cacheKey);
            if (Plan != null)
            {
                return Ok(Plan);
            }

            var Result = await Mediator.Send(new GetAllPlanQueryForUsers());
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, Result.Data, TimeSpan.FromMinutes(20));
            }

            return NewResult(Result);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [AllowAnonymous]
        [HttpGet("{Id}")]
        [SwaggerOperation(
            Summary = "Get plan by id",
            Description = "Retrieve subscription plan details by id"
        )]
        public async Task<IActionResult> GetById(int Id)
        {
            var Result = await Mediator.Send(new GetPlanByIdQuery(Id));
            return NewResult(Result);
        }

        #endregion

        #region Delete
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpDelete("{Id}")]
        [SwaggerOperation(Summary = "Delete plan", Description = "Delete subscription plan by id")]
        public async Task<IActionResult> Delete(int Id)
        {

            var Result = await Mediator.Send(new DeletePlanCommand(Id));
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }
        #endregion



        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
        }
    }
}

using LMS.API.Bases;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Feature.Subscriptions.Query.Model;
using LMS.Core.Feature.Subscriptions.Query.Model.AdminModel;
using LMS.Core.Feature.Subscriptions.Query.Model.StudentModel;
using LMS.Core.Feature.Subscriptions.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : AppBaseController
    {
        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_Subscription_user_{UserId}";
        private string GetAllMySubscriptioncacheKey => $"all_GetAllMySubscriptionSubscription_user_{UserId}";
        private string GetAllMyRequestsSubscriptioncacheKey => $"all_GetAllMyRequestsSubscription_user_{UserId}";


        #region Post 
        [Authorize]
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPost]
        [SwaggerOperation(Summary = "Add subscription", Description = "Add a new subscription for a user")]
        public async Task<IActionResult> Add([FromBody] AddSubscriptionsCommand addSubscriptionsCommand)
        {
            var Result = await Mediator.Send(addSubscriptionsCommand);
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }

        #endregion
        #region Put && Patch
        [Authorize]
        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpPatch("{Id}")]
        [SwaggerOperation(Summary = "Change subscription plan", Description = "Change the current subscription plan")]
        public async Task<IActionResult> Change([FromRoute] int Id, [FromBody] ChangeSubscriptionPlanCommand command)
        {
            command.SubscriptionId = Id;
            var Result = await Mediator.Send(command);
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }

        [Authorize]
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPut("{Id}/cancel")]
        [SwaggerOperation(
            Summary = "Cancel subscription",
            Description = "Cancel an active subscription"
        )]
        public async Task<IActionResult> Cancel(int Id)
        {
            var Result = await Mediator.Send(new CancelSubscriptionsCommand(Id));
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }

        #endregion

        #region Delete 
        [Authorize]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpDelete("{Id}")]
        [SwaggerOperation(Summary = "Delete subscription", Description = "Delete subscription by id")]
        public async Task<IActionResult> Delete(int Id)
        {
            var Result = await Mediator.Send(new DeleteSubscriptionsCommand(Id));
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }
        #endregion

        #region Get


        [Authorize]
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpGet("user")]

        [SwaggerOperation(
            Summary = "Get all user subscriptions",
            Description = "Retrieve My all subscriptions "
        )]
        public async Task<IActionResult> GetAllMySubscription()
        {

            var Result = await Mediator.Send(new GetAllMySubscriptionsQuery());

            return NewResult(Result);


        }
        [Authorize]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("user/requests")]
        [SwaggerOperation(
            Summary = "Get subscription requests",
            Description = "Retrieve all subscription requests for a user"
        )]
        public async Task<IActionResult> GetAllMyRequestsSubscriptions()
        {
            var Subscription = await RedisCacheService.GetDataAsync<List<GetAllMyRequestsSubscriptionsResult>>(GetAllMyRequestsSubscriptioncacheKey);
            if (Subscription != null)
            {
                return Ok(Subscription);
            }
            var Result = await Mediator.Send(new GetAllMyRequestsSubscriptionsQuery());
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(GetAllMyRequestsSubscriptioncacheKey, Result.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(Result);


        }
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all subscriptions",
            Description = "Retrieve all subscriptions in the system (Admin)"
        )]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var Subscription = await RedisCacheService.GetDataAsync<GetAllSubscriptionsResponse>(cacheKey);
            if (Subscription != null)
            {
                return Ok(Subscription);
            }

            var Result = await Mediator.Send(new GetAllSubscriptionsQuery());
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, Result.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(Result);
        }
        [Authorize]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("user/{UserId}/current")]
        public async Task<IActionResult> GetSubscriptionByUserId(string UserId)
        {
            var Result = await Mediator.Send(new GetSubscriptionByUserIdQuery(UserId));
            return NewResult(Result);
        }
        [Authorize]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("user/{UserId}/active")]
        [SwaggerOperation(
            Summary = "Get current subscription",
            Description = "Retrieve the current active subscription for a user"
        )]
        public async Task<IActionResult> Check(string UserId)
        {
            var Result = await Mediator.Send(new CheckActiveSubscriptionQuery(UserId));
            return NewResult(Result);

        }

        [Authorize(Roles = "Admin")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpGet("Summary")]
        [SwaggerOperation(
            Summary = "Subscription summary",
            Description = "Get subscription statistics and summary for admin dashboard"
        )]
        public async Task<IActionResult> Summary()
        {
            var Result = await Mediator.Send(new SummarySubscriptionQuery());
            return NewResult(Result);

        }
        #endregion

        [Authorize]
        [HttpPost("{id}/RenewSubscriptionRequest")]
        public async Task<IActionResult> RenewSubscriptionRequest(int id, [FromBody] RenewSubscriptionsCommand request)
        {
            var result = await Mediator.Send(new RenewSubscriptionsCommand
            {
                SubscriptionId = id,
                PlanId = request.PlanId,
                paymentMethod = request.paymentMethod
            });

            return NewResult(result);
        }
        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(GetAllMyRequestsSubscriptioncacheKey);
            await RedisCacheService.RemoveAsync(GetAllMySubscriptioncacheKey);

        }
    }
}

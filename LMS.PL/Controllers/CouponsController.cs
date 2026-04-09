using LMS.API.Bases;
using LMS.Core.Feature.Coupons.Command.Models;
using LMS.Core.Feature.Coupons.Query.Models;
using LMS.Core.Feature.Coupons.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CouponsController : AppBaseController
    {
        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_Coupons_user_{UserId}";
        private string PaginatedcacheKey => $"all_Paginated_user_{UserId}";
        private string ExpiredcacheKey => $"all_Expired_user_{UserId}";


        #region Post
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create coupon",
            Description = "Create a new discount coupon"
        )]
        public async Task<IActionResult> Create(CreateCouponCommand command)
        {
            var Result = await Mediator.Send(command);
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }



        #endregion


        #region Delete
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpDelete("{Id}")]
        [SwaggerOperation(
            Summary = "Delete coupon",
            Description = "Delete coupon by id"
        )]
        public async Task<IActionResult> Delete(int Id)
        {
            var Result = await Mediator.Send(new DeleteCouponCommand(Id));
            if (Result.Succeeded)
            {
                await ClearCache();
            }
            return NewResult(Result);
        }






        #endregion

        #region Put & Patch
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPut("{Id}")]
        [SwaggerOperation(
            Summary = "Update coupon",
            Description = "Update coupon information by id"
        )]
        public async Task<IActionResult> Update([FromRoute] int Id, UpdateCouponCommand command)
        {
            command.Id = Id;
            var Result = await Mediator.Send(command);
            if (Result.Succeeded)
            {
                // Invalidate the cache for all coupons when a new coupon is created
                await RedisCacheService.RemoveAsync(cacheKey);
            }
            return NewResult(Result);
        }
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPatch("ChangeStatus/{Id}")]
        [SwaggerOperation(
            Summary = "Change coupon status",
            Description = "Activate or deactivate a coupon"
        )]
        public async Task<IActionResult> ChangeStatus(int Id, ChangeStatusOfCouponCommand command)
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
        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all coupons",
            Description = "Retrieve all coupons with optional filtering or pagination"
        )]
        public async Task<IActionResult> GetAll([FromQuery] GetAllQuery query)
        {
            //var Coupon = await RedisCacheService.GetDataAsync<List<GetAllCouponResult>>(cacheKey);
            //if (Coupon != null)
            //{
            //    return Ok(Coupon);
            //}


            var Result = await Mediator.Send(query);
            //if (Result != null)
            //{
            //    await RedisCacheService.SetDataAsync(cacheKey, Result.Data, TimeSpan.FromMinutes(15));
            //}

            return NewResult(Result);

        }
        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpGet("Paginated")]
        [SwaggerOperation(
            Summary = " Get All Coupon Query Paginated",
            Description = "Retrieve all coupons with optional filtering or pagination"
        )]
        public async Task<IActionResult> GetAllCouponQueryPaginated([FromQuery] GetAllCouponQueryPaginated query)
        {
            var Coupon = await RedisCacheService.GetDataAsync<List<GetAllCouponQueryPaginatedResult>>(PaginatedcacheKey);
            if (Coupon != null)
            {
                return Ok(Coupon);
            }

            var Result = await Mediator.Send(query);
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(PaginatedcacheKey, Result.Data, TimeSpan.FromMinutes(15));
            }

            return Ok(Result);

        }

        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpGet("Expired")]
        [SwaggerOperation(
            Summary = "Get expired coupons",
            Description = "Retrieve all expired coupons"
        )]
        public async Task<IActionResult> GetAlExpired()
        {
            var Coupon = await RedisCacheService.GetDataAsync<List<GetAllExpiredResult>>(ExpiredcacheKey);
            if (Coupon != null) return Ok(Coupon);


            var Result = await Mediator.Send(new GetAllExpiredQuery());
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(ExpiredcacheKey, Result.Data, TimeSpan.FromMinutes(15));
            }
            return NewResult(Result);

        }

        [EnableRateLimiting("SlidingWindowPolicy")]
        [HttpGet("ByCode/{Code}")]
        [SwaggerOperation(
            Summary = "Get coupon by code",
            Description = "Retrieve coupon details using coupon code"
        )]
        public async Task<IActionResult> ByCode(string Code)
        {

            var Result = await Mediator.Send(new GetByCodeQuery(Code));
            return NewResult(Result);

        }

        #endregion

        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(PaginatedcacheKey);
            await RedisCacheService.RemoveAsync(ExpiredcacheKey);

        }


    }
}

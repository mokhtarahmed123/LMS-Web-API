using LMS.API.Bases;
using LMS.Core.Feature.Courses.Command.Models.AdminCommand;
using LMS.Core.Feature.Courses.Command.Models.InstructorCommand;
using LMS.Core.Feature.Courses.Query.Models;
using LMS.Core.Feature.Courses.Query.Models.AdminModel;
using LMS.Core.Feature.Courses.Query.Models.InstructorModel;
using LMS.Core.Feature.Courses.Query.Result;
using LMS.Core.Feature.Courses.Query.Result.AdminResultQuery;
using LMS.Core.Feature.Courses.Query.Result.InstructorResultQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : AppBaseController
    {
        private readonly IWebHostEnvironment _env;
        private string UserId => CurrentUserService.UserId;
        private string cacheKey => $"all_Course_user_{UserId}";
        private string CategoryIdcacheKey => $"all_ByCategoryId_user_{UserId}";
        private string CoursesByInstructorIdcacheKey => $"all_CoursesByInstructorIdcacheKey_user_{UserId}";
        private string GetMyCoursescacheKey => $"all_GetMyCoursescacheKey_user_{UserId}";
        private string CoursesPendingcacheKey => $"all_CoursesPendingcacheKey_user_{UserId}";
        private string AllCoursesPaginatedcacheKey => $"all_AllCoursesPaginatedcacheKey_user_{UserId}";


        public CourseController(IWebHostEnvironment env)
        {
            this._env = env;
        }

        #region Post
        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost]

        [SwaggerOperation(
            Summary = "Add new course",
            Description = "Create a new course with course details and files"
        )]
        public async Task<IActionResult> Add([FromForm] AddCourseCommand command)
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
        [HttpGet]

        [SwaggerOperation(
            Summary = "Get all courses",
            Description = "Retrieve all available courses"
        )]
        public async Task<IActionResult> GetAll()
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetAllCoursesResult>>(cacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }
            var response = await Mediator.Send(new GetAllCoursesQuery());
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(cacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get course by id",
            Description = "Retrieve course details by course id"
        )]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await Mediator.Send(new GetCourseByIdQuery(id));
            return NewResult(response);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("category/{CategoryId}")]
        [SwaggerOperation(
            Summary = "Get courses by category",
            Description = "Retrieve all courses for a specific category"
        )]
        public async Task<IActionResult> GetAllByCategoryId(int CategoryId)
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetAllCoursesByCategoryIdResult>>(CategoryIdcacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }


            var response = await Mediator.Send(new GetAllCoursesByCategoryIdQuery(CategoryId));
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(CategoryIdcacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }

        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("instructor/{InstructorId}")]
        [SwaggerOperation(
            Summary = "Get courses by instructor",
            Description = "Retrieve all courses created by a specific instructor"
        )]
        public async Task<IActionResult> GetAllCoursesByInstructorId(int InstructorId)
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetAllCoursesByInstructorIdResult>>(CoursesByInstructorIdcacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }


            var response = await Mediator.Send(new GetAllCoursesByInstructorIdQuery(InstructorId));
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(CoursesByInstructorIdcacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [Authorize(Roles = "Instructor")]
        [HttpGet("GetMyCourses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetMyCoursesResult>>(GetMyCoursescacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }

            var response = await Mediator.Send(new GetMyCoursesQuery());
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(GetMyCoursescacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [Authorize(Roles = "Admin")]
        [HttpGet("CoursesPending")]
        public async Task<IActionResult> CoursesPending()
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetAllCoursesPendingResult>>(CoursesPendingcacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }
            var response = await Mediator.Send(new GetAllCoursesPendingQuery());
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(CoursesPendingcacheKey, response.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(response);
        }


        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpGet("Paginated")]
        public async Task<IActionResult> GetAllCoursesPaginatedResult([FromQuery] GetAllCoursesPaginatedQuery query)
        {
            var Course = await RedisCacheService.GetDataAsync<List<GetAllCoursesPaginatedResult>>(AllCoursesPaginatedcacheKey);
            if (Course != null)
            {
                return Ok(Course);
            }

            var response = await Mediator.Send(query);
            if (response != null)
            {
                await RedisCacheService.SetDataAsync(AllCoursesPaginatedcacheKey, response.Data, TimeSpan.FromMinutes(20));
            }

            return Ok(response);
        }
        #endregion

        #region Put && Patch

        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update course", Description = "Update course details")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCourseCommand command)
        {
            command.CourseId = id;
            var response = await Mediator.Send(command);
            if (response != null) await ClearCache();
            return NewResult(response);
        }

        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPatch("reject")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Reject course",
            Description = "Admin rejects the course"
        )]

        public async Task<IActionResult> Reject(RejectCourseCommand command)
        {
            var response = await Mediator.Send(command);
            if (response != null) await ClearCache();

            return NewResult(response);
        }

        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/approve")]
        [SwaggerOperation(
            Summary = "Approve course",
            Description = "Admin approves the course"
        )]
        public async Task<IActionResult> Approve(int id)
        {
            var response = await Mediator.Send(new ApproveCourseCommand(id));
            if (response != null) await ClearCache();


            return NewResult(response);
        }

        #endregion

        #region Delete
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpDelete("{Id}/admin")]
        [Authorize(Roles = "Admin")]

        [SwaggerOperation(
        Summary = "Delete course by admin",
        Description = "Allow admin to delete any course"
    )]
        public async Task<IActionResult> DeleteByAdmin(int Id)
        {
            var response = await Mediator.Send(new DeleteCourseByAdminCommand(Id));
            if (response != null) await ClearCache();

            return NewResult(response);

        }


        [HttpDelete("{Id}/instructor")]
        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize(Roles = "Instructor")]

        [SwaggerOperation(
            Summary = "Delete course by instructor",
            Description = "Allow instructor to delete their own course"
        )]
        public async Task<IActionResult> DeleteByInstructor(int Id)
        {
            var response = await Mediator.Send(new DeleteCourseByInstructorCommand(Id));
            if (response != null) await ClearCache();

            return NewResult(response);

        }
        #endregion

        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(CategoryIdcacheKey);
            await RedisCacheService.RemoveAsync(CoursesByInstructorIdcacheKey);
            await RedisCacheService.RemoveAsync(GetMyCoursescacheKey);
            await RedisCacheService.RemoveAsync(CoursesPendingcacheKey);
            await RedisCacheService.RemoveAsync(AllCoursesPaginatedcacheKey);
        }



    }
}
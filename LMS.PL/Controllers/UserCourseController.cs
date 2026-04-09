using LMS.API.Bases;
using LMS.Core.Feature.UserCourses.Command.Models.Student;
using LMS.Core.Feature.UserCourses.Query.Models.AdminModel;
using LMS.Core.Feature.UserCourses.Query.Models.StudentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserCourseController : AppBaseController
    {
        private string UserId => CurrentUserService.UserId;

        private string cacheKey => $"all_UserCourse_user_{UserId}";
        private string AllStudentByCourseIdcacheKey => $"all_AllStudentByCourseIdcacheKey_user_{UserId}";
        private string FavoritecacheKey => $"all_FavoriteUserCourse_user_{UserId}";


        #region Post
        [HttpPost("enroll")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [SwaggerOperation(Summary = "Enroll in course", Description = "Allow a user to enroll in a course")]
        public async Task<IActionResult> EnrollmentCourse([FromBody] EnrollUserCourseCommand enrollCourseCommand)
        {
            var Result = await Mediator.Send(enrollCourseCommand);
            if (Result.Succeeded) await ClearCache();
            return NewResult(Result);
        }



        #endregion
        #region Delete
        [HttpDelete("unenroll")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [SwaggerOperation(Summary = "Unenroll from course", Description = "Allow a user to remove enrollment from a course")]
        public async Task<IActionResult> UnenrollCourse([FromQuery] UnenrollCourseCommand UnenrollCourseCommand)
        {
            var Result = await Mediator.Send(UnenrollCourseCommand);
            if (Result.Succeeded) await ClearCache();

            return NewResult(Result);
        }



        #endregion


        #region Put && Patch
        [HttpPatch("rate")]
        [EnableRateLimiting("TokenBucketPolicy")]

        [SwaggerOperation(Summary = "Rate course", Description = "Allow a user to rate a course after enrollment")]
        public async Task<IActionResult> RateCourse([FromBody] RateUserCourseCommand UnenrollCourseCommand)
        {
            var Result = await Mediator.Send(UnenrollCourseCommand);


            return NewResult(Result);
        }

        [HttpPatch("favourite")]
        [EnableRateLimiting("TokenBucketPolicy")]

        [SwaggerOperation(
            Summary = "Add or remove favourite course",
            Description = "Allow a user to mark or unmark a course as favourite"
        )]
        public async Task<IActionResult> FavouriteCourse([FromQuery] favouriteUserCourseCommand UnenrollCourseCommand)
        {
            var Result = await Mediator.Send(UnenrollCourseCommand);


            return NewResult(Result);
        }
        #endregion

        #region Get


        [HttpGet("user/enrollments")]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(
            Summary = "Get user enrollments",
            Description = "Retrieve all courses the user is enrolled in"
        )]
        public async Task<IActionResult> GetAllCoursesEnrollmentsByUserId()
        {

            var Result = await Mediator.Send(new GetAllMyCoursesEnrollmentsQuery());


            return NewResult(Result);
        }

        [HttpGet("user/favorites")]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(
            Summary = "Get My favourite courses",
            Description = "Retrieve all My favourite courses for the user"
        )]
        public async Task<IActionResult> GetMyFavouriteCoursesByUserId()
        {


            var Result = await Mediator.Send(new GetMyFavouriteCoursesByUserIdQuery());


            return NewResult(Result);
        }
        [HttpGet("{CourseId}/students")]
        [EnableRateLimiting("SlidingWindowPolicy")]

        [SwaggerOperation(
            Summary = "Get All Student Enroll This Course   ",
            Description = "Get All Student Enroll This Course"
        )]
        public async Task<IActionResult> GetAllStudentByCourseId(int CourseId)
        {


            var Result = await Mediator.Send(new GetAllStudentByCourseIdQuery(CourseId));

            return NewResult(Result);
        }

        #endregion



        private async Task ClearCache()
        {

            await RedisCacheService.RemoveAsync(cacheKey);
            await RedisCacheService.RemoveAsync(FavoritecacheKey);
            await RedisCacheService.RemoveAsync(AllStudentByCourseIdcacheKey);
        }
    }
}
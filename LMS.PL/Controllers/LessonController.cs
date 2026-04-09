using LMS.API.Bases;
using LMS.Core.Feature.Lessons.Command.Models;
using LMS.Core.Feature.Lessons.Query.Models;
using LMS.Core.Feature.Lessons.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : AppBaseController
    {

        private string UserId => CurrentUserService.UserId;
        private string cacheKeyWithLesson => $"all_Lesson_user_{UserId}";

        #region Post

        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("TokenBucketPolicy")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Add new lesson",
            Description = "Create a new lesson for a specific course"
        )]
        public async Task<IActionResult> Add(AddLessonCommand addLessonCommand)
        {
            var Result = await Mediator.Send(addLessonCommand);
            if (Result.Succeeded)
            {
                await ClearCacheWithLesson();
            }
            return NewResult(Result);

        }

        #endregion

        #region Put
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update lesson",
            Description = "Update lesson information"
        )]
        public async Task<IActionResult> Update(UpdateLessonCommand updateLessonCommand)
        {
            var result = await Mediator.Send(updateLessonCommand);
            if (result.Succeeded)
            {
                await ClearCacheWithLesson();
            }

            return NewResult(result);
        }

        [HttpPatch("Reorder")]
        [SwaggerOperation(
            Summary = "Reorder lessons",
            Description = "Change the order of lessons within a course"
        )]
        public async Task<IActionResult> ReorderLessons(ReorderLessonCommand command)
        {
            var result = await Mediator.Send(command);
            if (result.Succeeded)
            {
                await ClearCacheWithLesson();
            }
            return NewResult(result);
        }


        #endregion

        #region Delete
        [EnableRateLimiting("TokenBucketPolicy")]

        [HttpDelete("{Id}")]
        [SwaggerOperation(Summary = "Delete lesson", Description = "Delete lesson by lesson id")]

        public async Task<IActionResult> Delete(int Id)
        {
            var Result = await Mediator.Send(new DeleteLessonCommand(Id));
            if (Result.Succeeded)
            {
                await ClearCacheWithLesson();
            }
            return NewResult(Result);
        }
        #endregion


        #region Get
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("course/{courseId}")]
        [SwaggerOperation(
            Summary = "Get lessons by course id",
            Description = "Retrieve all lessons belonging to a specific course"
        )]
        public async Task<IActionResult> GetAllLessonsByCourseId(int courseId)
        {
            var Lesson = await RedisCacheService.GetDataAsync<List<GetAllLessonsByCourseIdResult>>(cacheKeyWithLesson);
            if (Lesson != null)
            {
                return Ok(Lesson);
            }

            var Result = await Mediator.Send(new GetLessonsByCourseIdQuery(courseId));
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(cacheKeyWithLesson, Result.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(Result);
        }

        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var Result = await Mediator.Send(new GetLessonByIdQuery(Id));
            return NewResult(Result);
        }
        #endregion









        private async Task ClearCacheWithLesson()
        {

            await RedisCacheService.RemoveAsync(cacheKeyWithLesson);
        }


    }
}

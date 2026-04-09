using LMS.API.Bases;
using LMS.Core.Feature.LessonFiles.Command.Models;
using LMS.Core.Feature.LessonFiles.Query.Models;
using LMS.Core.Feature.LessonFiles.Query.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileLessonController : AppBaseController
    {
        private string UserId => CurrentUserService.UserId;
        private string cacheKeyWithFile => $"all_LessonFile_user_{UserId}";


        #region Files 


        #region Post



        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("FixedWindowPolicy")]
        [HttpPost("Upload")]
        public async Task<IActionResult> AddFile(AddLessonsFileCommand command)
        {
            var Result = await Mediator.Send(command);
            //if (Result.Succeeded)
            //{
            //    await ClearCacheWithFile();
            //}

            return NewResult(Result);


        }



        #endregion

        #region Get
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("Files/{LessonId}")]
        public async Task<IActionResult> GetAllFilesByLessonId(int LessonId)
        {
            var LessonFile = await RedisCacheService.GetDataAsync<List<AllLessonFileResult>>(cacheKeyWithFile);
            if (LessonFile != null)
            {
                return Ok(LessonFile);
            }

            var Result = await Mediator.Send(new GetAllFilesByLessonIdQuery(LessonId));
            if (Result != null)
            {
                await RedisCacheService.SetDataAsync(cacheKeyWithFile, Result.Data, TimeSpan.FromMinutes(20));
            }
            return NewResult(Result);
        }
        [EnableRateLimiting("SlidingWindowPolicy")]

        [HttpGet("File/{Id}")]
        public async Task<IActionResult> GetFileById(int Id)
        {
            var Result = await Mediator.Send(new GetFileByIdQuery(Id));
            return NewResult(Result);
        }
        #endregion


        #region Put && Patch

        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpPatch("EditFile/{Id}")]
        public async Task<IActionResult> EditFile(int Id, EditLessonsFileCommand command)
        {

            command.FileId = Id;
            var Result = await Mediator.Send(command);
            if (Result.Succeeded)
            {
                await ClearCacheWithFile();
            }
            return NewResult(Result);
        }
        #endregion
        #region Delete

        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("FixedWindowPolicy")]

        [HttpDelete("DeleteFile/{Id}")]
        public async Task<IActionResult> DeleteFile(int Id)
        {
            var Result = await Mediator.Send(new DeleteLessonsFileCommand(Id));
            //if (Result.Succeeded)
            //{
            //    await ClearCacheWithFile();
            //}
            return NewResult(Result);
        }
        #endregion




        #endregion



        private async Task ClearCacheWithFile()
        {

            await RedisCacheService.RemoveAsync(cacheKeyWithFile);
        }


    }
}

using LMS.API.Bases;
using LMS.Core.Feature.QuestionOptions.Command.Model;
using LMS.Core.Feature.QuestionOptions.Query.Model;
using LMS.Core.Feature.QuizQuestions.Command.Model;
using LMS.Core.Feature.QuizQuestions.Query.Model;
using LMS.Core.Feature.QuizSubmissions.Command.Model;
using LMS.Core.Feature.QuizSubmissions.Query.Model;
using LMS.Core.Feature.Quizzes.Command.Model;
using LMS.Core.Feature.Quizzes.Query.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : AppBaseController
    {

        #region Quiz

        #region Post
        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> AddQuiz([FromBody] AddQuizCommand command)
        {
            var result = await Mediator.Send(command);
            return NewResult(result);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("FixedWindowPolicy")]

        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var result = await Mediator.Send(new DeleteQuizCommand(id));
            return NewResult(result);
        }
        #endregion

        #region Put && Patch
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor")]
        [EnableRateLimiting("FixedWindowPolicy")]

        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] UpdateQuizCommand command)
        {
            command.Id = id;

            var result = await Mediator.Send(command);
            return NewResult(result);
        }


        #endregion

        #region Get
        [EnableRateLimiting("TokenBucketPolicy")]
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            var result = await Mediator.Send(new GetAllQuizzesQuery());
            return NewResult(result);
        }
        [EnableRateLimiting("TokenBucketPolicy")]
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("{courseId}/{id}")]
        public async Task<IActionResult> GetQuizById(int id, int courseId)
        {
            var result = await Mediator.Send(new GetQuizByIdQuery(id, courseId));
            return NewResult(result);
        }
        [EnableRateLimiting("TokenBucketPolicy")]
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetAllQuizzesByCourseId(int courseId)
        {
            var result = await Mediator.Send(new GetAllQuizzesByCourseIdQuery(courseId));
            return NewResult(result);
        }
        #endregion

        #endregion


        #region QuizQuestion

        #region Post
        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize(Roles = "Instructor")]
        [HttpPost("{quizId}/questions")]
        public async Task<IActionResult> AddQuizQuestion(int quizId, [FromBody] AddQuizQuestionsCommand command)
        {
            command.QuizId = quizId;
            var result = await Mediator.Send(command);
            return NewResult(result);
        }
        #endregion

        #region Put && Patch
        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize(Roles = "Instructor")]
        [HttpPut("{quizId}/questions/{id}")]
        public async Task<IActionResult> UpdateQuizQuestion(int quizId, int id, [FromBody] UpdateQuizQuestionsCommand command)
        {
            command.QuizId = quizId;
            command.Id = id;
            var result = await Mediator.Send(command);
            return NewResult(result);
        }
        #endregion

        #region Delete
        [EnableRateLimiting("FixedWindowPolicy")]
        [Authorize(Roles = "Instructor")]
        [HttpDelete("questions/{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(int id)
        {
            var result = await Mediator.Send(new DeleteQuizQuestionsCommand(id));
            return NewResult(result);
        }
        #endregion

        #region Get
        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("{quizId}/questions")]

        public async Task<IActionResult> GetAllquestionsByQuizId(int quizId)
        {
            var result = await Mediator.Send(new GetQuizQuestionsByQuizIdQuery(quizId));
            return NewResult(result);
        }

        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("questions/{Id}")]
        public async Task<IActionResult> GetquestionById(int Id)
        {
            var result = await Mediator.Send(new GetQuizQuestionByIdQuery(Id));
            return NewResult(result);

        }
        #endregion



        #endregion


        #region QuestionOption
        #region Post


        [EnableRateLimiting("FixedWindowPolicy")]

        [Authorize(Roles = "Instructor")]
        [HttpPost("{questionId}/options")]
        public async Task<IActionResult> AddQuestionOption(int questionId, [FromBody] AddQuestionOptionsCommand command)
        {
            command.QuizQuestionId = questionId;
            var result = await Mediator.Send(command);
            return NewResult(result);
        }

        #endregion

        #region Delete

        [EnableRateLimiting("FixedWindowPolicy")]

        [Authorize(Roles = "Instructor")]
        [HttpDelete("option/{Id}")]
        public async Task<IActionResult> DeleteQuestionOption(int Id)
        {
            var Result = await Mediator.Send(new DeleteQuestionOptionsCommand(Id));
            return NewResult(Result);
        }
        #endregion

        #region Put && Patch
        [EnableRateLimiting("FixedWindowPolicy")]

        [Authorize(Roles = "Instructor")]
        [HttpPatch("option/{Id}")]
        public async Task<IActionResult> UpdateQuestionOption([FromRoute] int Id, [FromBody] UpdateQuestionOptionsCommand command)
        {
            command.Id = Id;
            var Result = await Mediator.Send(command);
            return NewResult(Result);
        }
        #endregion

        #region Get
        [EnableRateLimiting("TokenBucketPolicy")]

        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("option/{Id}")]
        public async Task<IActionResult> GetOptionById(int Id)
        {
            var Result = await Mediator.Send(new GetQuestionOptionsByIdQuery(Id));
            return NewResult(Result);
        }
        [EnableRateLimiting("TokenBucketPolicy")]
        [Authorize(Roles = "Instructor,Student")]
        [HttpGet("{questionId}/options")]
        public async Task<IActionResult> GetQuestionOptionsByQuestionId(int questionId)
        {
            var result = await Mediator.Send(new GetQuestionOptionsByQuestionIdQuery(questionId));
            return NewResult(result);
        }
        #endregion

        #endregion

        [Authorize(Roles = "Student")]
        [HttpPost("Submit/{quizId}")]
        public async Task<IActionResult> SubmitQuiz(int quizId, [FromBody] SubmitQuizCommand command)
        {
            command.QuizId = quizId;
            var result = await Mediator.Send(command);
            return NewResult(result);
        }
        [Authorize(Roles = "Student")]
        [HttpGet("MySubmissions")]
        public async Task<IActionResult> MySubmissions()
        {
            var result = await Mediator.Send(new GetAllMySubmissionsQuery());
            return NewResult(result);
        }


    }
}
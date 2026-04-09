using LMS.Core.Bases;
using LMS.Core.Feature.QuizSubmissions.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Entities.Quiz;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuizSubmissions.Command.Handler
{
    public class QuizSubmissionsCommandHandler : ResponseHandler, IRequestHandler<SubmitQuizCommand, Response<string>>
    {
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IQuizSubmissionsService quizSubmissionsService;
        private readonly IQuizService quizService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserCoursesService userCoursesService;
        private readonly IQuestionOptionsService questionOptionsService;
        private readonly IQuizQuestionService quizQuestionService;

        public QuizSubmissionsCommandHandler(UserManager<Users> userManager, RoleManager<Role> roleManager, IQuizSubmissionsService quizSubmissionsService, IQuizService quizService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, IUserCoursesService userCoursesService, IQuestionOptionsService QuestionOptionsService, IQuizQuestionService quizQuestionService) : base(stringLocalizer)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.quizSubmissionsService = quizSubmissionsService;
            this.quizService = quizService;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
            this.userCoursesService = userCoursesService;
            questionOptionsService = QuestionOptionsService;
            this.quizQuestionService = quizQuestionService;
        }


        public async Task<Response<string>> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizService.GetQuizByIdAsync(request.QuizId);
            if (quiz == null)
                return NotFound<string>("Quiz not found.");

            // Authorize course access
            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null)
                return authResult;

            // Validate QuestionIds belong to this Quiz
            var validQuestions = await quizQuestionService.GetQuestionsByQuizIdAsync(request.QuizId);
            var validQuestionIds = validQuestions.Select(q => q.Id).ToList(); // ✅ حوّلها لـ List<int>

            var invalidAnswers = request.answersQuizzes
                .Where(a => !validQuestionIds.Contains(a.QuestionId))
                .ToList();

            if (invalidAnswers.Any())
                return BadRequest<string>($"Invalid QuestionIds: {string.Join(", ", invalidAnswers.Select(a => a.QuestionId))}");


            int score = request.answersQuizzes
                .Count(a => validQuestions
                    .FirstOrDefault(q => q.Id == a.QuestionId)
                    ?.Options
                    .Any(o => o.Id == a.SelectOption && o.IsCorrect) == true);

            var submission = new LMS.Data_.Entities.Quiz.QuizSubmissions
            {
                QuizId = request.QuizId,
                UserId = currentUserService.UserId,
                SubmittedAt = DateTime.UtcNow,
                Score = score
            };

            var answers = request.answersQuizzes.Select(a => new SubmissionAnswers
            {
                QuestionId = a.QuestionId,
                SelectedOptionId = a.SelectOption
            }).ToList();

            var result = await quizSubmissionsService.SubmitQuizAsync(submission, answers);

            return result != null
                ? Success<string>($"Quiz submitted successfully. Your score is {score}. , {result}")
                : BadRequest<string>("Failed to submit quiz.");
        }

        private async Task<Response<T>?> AuthorizeCourseAccessAsync<T>(int courseId)
        {
            var userId = currentUserService.UserId;
            var user = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Student")
            {
                var userCourses = await userCoursesService.GetUserCoursesByUserIdAsync(userId);
                if (!userCourses.Any(c => c.CourseId == courseId))
                    return Unauthorized<T>();
            }

            return null;
        }
    }
}

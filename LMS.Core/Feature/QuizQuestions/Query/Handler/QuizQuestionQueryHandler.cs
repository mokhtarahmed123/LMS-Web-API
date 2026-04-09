using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.QuizQuestions.Query.Model;
using LMS.Core.Feature.QuizQuestions.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuizQuestions.Query.Handler
{
    public class QuizQuestionQueryHandler : ResponseHandler,
        IRequestHandler<GetQuizQuestionsByQuizIdQuery, Response<List<GetQuizQuestionsByQuizIdResult>>>
        , IRequestHandler<GetQuizQuestionByIdQuery, Response<GetQuizQuestionByIdResult>>
    {
        private readonly IMapper mapper;
        private readonly IQuizQuestionService quizQuestionService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IUserCoursesService userCoursesService;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ICoursesService coursesService;
        private readonly IQuizService quizService;

        public QuizQuestionQueryHandler(IMapper mapper, IQuizQuestionService quizQuestionService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, UserManager<Users> userManager, RoleManager<Role> roleManager, IUserCoursesService userCoursesService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService, IQuizService quizService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.quizQuestionService = quizQuestionService;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userCoursesService = userCoursesService;
            this.instructorProfilesService = instructorProfilesService;
            this.coursesService = coursesService;
            this.quizService = quizService;
        }


        public async Task<Response<List<GetQuizQuestionsByQuizIdResult>>> Handle(GetQuizQuestionsByQuizIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await quizService.GetQuizByIdAsync(request.QuizId);
            if (quiz == null)
                return NotFound<List<GetQuizQuestionsByQuizIdResult>>();

            var authResult = await AuthorizeCourseAccessAsync<List<GetQuizQuestionsByQuizIdResult>>(quiz.CourseId);
            if (authResult != null) return authResult;

            var questions = await quizQuestionService.GetQuestionsByQuizIdAsync(request.QuizId);
            if (questions == null || !questions.Any())
                return NotFound<List<GetQuizQuestionsByQuizIdResult>>();
            var Mapped = mapper.Map<List<GetQuizQuestionsByQuizIdResult>>(questions);
            var total = questions.Count;

            return Success(Mapped, $"Total = {total} ");
        }

        public async Task<Response<GetQuizQuestionByIdResult>> Handle(GetQuizQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(request.Id);
            if (quizQuestion == null)
                return NotFound<GetQuizQuestionByIdResult>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<GetQuizQuestionByIdResult>();

            var authResult = await AuthorizeCourseAccessAsync<GetQuizQuestionByIdResult>(quiz.CourseId);
            if (authResult != null) return authResult;


            return Success(mapper.Map<GetQuizQuestionByIdResult>(quizQuestion));
        }

        private async Task<Response<T>?> AuthorizeCourseAccessAsync<T>(int courseId)
        {
            var userId = currentUserService.UserId;
            var user = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            if (role == "Instructor")
            {
                var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
                if (instructorProfile == null)
                    return NotFound<T>();

                var instructorCourses = await coursesService.GetCoursesByInstructorIdAsync(instructorProfile.Id);
                if (!instructorCourses.Any(c => c.Id == courseId))
                    return Unauthorized<T>();
            }
            else if (role == "Student")
            {
                var userCourses = await userCoursesService.GetUserCoursesByUserIdAsync(userId);
                if (!userCourses.Any(c => c.CourseId == courseId))
                    return Unauthorized<T>();
            }

            return null;
        }
    }
}

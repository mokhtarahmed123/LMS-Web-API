using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.QuizSubmissions.Query.Model;
using LMS.Core.Feature.QuizSubmissions.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuizSubmissions.Query.Handler
{
    public class QuizSubmissionsQueryHandler : ResponseHandler, IRequestHandler<GetAllMySubmissionsQuery, Response<List<GetAllMySubmissionsResult>>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IQuizSubmissionsService quizSubmissionsService;
        private readonly IQuizService quizService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserCoursesService userCoursesService;
        private readonly IQuestionOptionsService questionOptionsService;
        private readonly IQuizQuestionService quizQuestionService;

        public QuizSubmissionsQueryHandler(IMapper mapper, UserManager<Users> userManager, RoleManager<Role> roleManager, IQuizSubmissionsService quizSubmissionsService, IQuizService quizService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, IUserCoursesService userCoursesService, IQuestionOptionsService QuestionOptionsService, IQuizQuestionService quizQuestionService) : base(stringLocalizer)
        {
            this.mapper = mapper;
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


        public async Task<Response<List<GetAllMySubmissionsResult>>> Handle(GetAllMySubmissionsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var submissions = await quizSubmissionsService.GetAllSubmissionAnswers(userId);

            if (submissions == null || !submissions.Any())
                return NotFound<List<GetAllMySubmissionsResult>>("No submissions found.");

            var answers = submissions
                .SelectMany(s => s.Answers)
                .ToList();

            var result = mapper.Map<List<GetAllMySubmissionsResult>>(answers);

            return Success(result);
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

using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.QuestionOptions.Query.Model;
using LMS.Core.Feature.QuestionOptions.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuestionOptions.Query.Handler
{
    public class QuestionOptionsQueryHandler : ResponseHandler,
        IRequestHandler<GetQuestionOptionsByQuestionIdQuery, Response<List<GetQuestionOptionsByQuestionIdResult>>>,
        IRequestHandler<GetQuestionOptionsByIdQuery, Response<GetQuestionOptionsByIdResult>>
    {


        private readonly IQuestionOptionsService questionOptionsService;
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

        public QuestionOptionsQueryHandler(IQuestionOptionsService questionOptionsService, IMapper mapper, IQuizQuestionService quizQuestionService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, UserManager<Users> userManager, RoleManager<Role> roleManager, IUserCoursesService userCoursesService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService, IQuizService quizService) : base(stringLocalizer)
        {
            this.questionOptionsService = questionOptionsService;
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
        public async Task<Response<List<GetQuestionOptionsByQuestionIdResult>>> Handle(GetQuestionOptionsByQuestionIdQuery request, CancellationToken cancellationToken)
        {
            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(request.QuestionId);
            if (quizQuestion == null)
                return NotFound<List<GetQuestionOptionsByQuestionIdResult>>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<List<GetQuestionOptionsByQuestionIdResult>>();

            var authResult = await AuthorizeCourseAccessAsync<List<GetQuestionOptionsByQuestionIdResult>>(quiz.CourseId);
            if (authResult != null) return authResult;

            var options = await questionOptionsService.GetOptionsByQuestionIdAsync(request.QuestionId);
            if (options == null || !options.Any())
                return NotFound<List<GetQuestionOptionsByQuestionIdResult>>();
            var Mapped = mapper.Map<List<GetQuestionOptionsByQuestionIdResult>>(options);
            return Success(Mapped, $"Total = {options.Count}");
        }

        public async Task<Response<GetQuestionOptionsByIdResult>> Handle(GetQuestionOptionsByIdQuery request, CancellationToken cancellationToken)
        {
            var option = await questionOptionsService.GetOptionByIdAsync(request.Id);
            if (option == null)
                return NotFound<GetQuestionOptionsByIdResult>();

            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(option.QuizQuestionId);
            if (quizQuestion == null)
                return NotFound<GetQuestionOptionsByIdResult>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<GetQuestionOptionsByIdResult>();

            var authResult = await AuthorizeCourseAccessAsync<GetQuestionOptionsByIdResult>(quiz.CourseId);
            if (authResult != null) return authResult;

            return Success(mapper.Map<GetQuestionOptionsByIdResult>(option));
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

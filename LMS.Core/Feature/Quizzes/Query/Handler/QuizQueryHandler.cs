using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Quizzes.Query.Model;
using LMS.Core.Feature.Quizzes.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Quizzes.Query.Handler
{
    public class QuizQueryHandler : ResponseHandler,
        IRequestHandler<GetAllQuizzesQuery, Response<List<GetAllQuizzesResult>>>,
        IRequestHandler<GetAllQuizzesByCourseIdQuery, Response<List<GetAllQuizzesByCourseIdResult>>>,
      IRequestHandler<GetQuizByIdQuery, Response<GetQuizByIdResult>>
    {
        private readonly IMapper mapper;
        private readonly IQuizService quizService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IUserCoursesService userCoursesService;

        public QuizQueryHandler(IMapper mapper, IQuizService quizService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService, UserManager<Users> userManager, RoleManager<Role> roleManager, IUserCoursesService userCoursesService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.quizService = quizService;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
            this.instructorProfilesService = instructorProfilesService;
            this.coursesService = coursesService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userCoursesService = userCoursesService;
        }


        public async Task<Response<List<GetAllQuizzesResult>>> Handle(GetAllQuizzesQuery request, CancellationToken cancellationToken)
        {
            var quizzes = quizService.GetAllQuizzesAsync();
            if (quizzes == null || !quizzes.Any())
            {
                return NotFound<List<GetAllQuizzesResult>>();
            }
            var result = mapper.Map<List<GetAllQuizzesResult>>(quizzes);
            return Success(result);


        }

        public async Task<Response<List<GetAllQuizzesByCourseIdResult>>> Handle(GetAllQuizzesByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var authResult = await AuthorizeCourseAccessAsync<List<GetAllQuizzesByCourseIdResult>>(request.CourseId);
            if (authResult != null) return authResult;

            var quizzes = await quizService.GetAllQuizzesByCourseIdAsync(request.CourseId);
            if (quizzes == null || !quizzes.Any())
                return NotFound<List<GetAllQuizzesByCourseIdResult>>();

            return Success(mapper.Map<List<GetAllQuizzesByCourseIdResult>>(quizzes));
        }

        public async Task<Response<GetQuizByIdResult>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var authResult = await AuthorizeCourseAccessAsync<GetQuizByIdResult>(request.CourseId);
            if (authResult != null) return authResult;

            var quiz = await quizService.GetQuizByIdAsync(request.QuizId);
            if (quiz == null)
                return NotFound<GetQuizByIdResult>();

            return Success(mapper.Map<GetQuizByIdResult>(quiz));
        }

        private async Task<Response<T>?> AuthorizeCourseAccessAsync<T>(int courseId)
        {
            var userId = currentUserService.UserId;
            var user = await userManager.FindByIdAsync(userId);
            var roles = await userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault();

            if (roleName == "Instructor")
            {
                var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
                if (instructorProfile == null)
                    return NotFound<T>();

                var instructorCourses = await coursesService.GetCoursesByInstructorIdAsync(instructorProfile.Id);
                if (!instructorCourses.Any(c => c.Id == courseId))
                    return Unauthorized<T>();
            }
            else if (roleName == "Student")
            {
                var userCourses = await userCoursesService.GetUserCoursesByUserIdAsync(userId);
                if (!userCourses.Any(c => c.CourseId == courseId))
                    return Unauthorized<T>();
            }

            return null;
        }
    }
}

using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.QuizQuestions.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuizQuestions.Command.Handler
{
    public class QuizQuestionsCommandHandler : ResponseHandler,
        IRequestHandler<AddQuizQuestionsCommand, Response<string>>
    , IRequestHandler<UpdateQuizQuestionsCommand, Response<string>>,
     IRequestHandler<DeleteQuizQuestionsCommand, Response<string>>
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

        public QuizQuestionsCommandHandler(IMapper mapper, IQuizQuestionService quizQuestionService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, UserManager<Users> userManager, RoleManager<Role> roleManager, IUserCoursesService userCoursesService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService, IQuizService quizService) : base(stringLocalizer)
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





        public async Task<Response<string>> Handle(AddQuizQuestionsCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizService.GetQuizByIdAsync(request.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            var quizQuestion = mapper.Map<LMS.Data_.Entities.Quiz.QuizQuestions>(request);
            var result = await quizQuestionService.AddQuestionAsync(quizQuestion);

            return result != null
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Created])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
        }

        public async Task<Response<string>> Handle(UpdateQuizQuestionsCommand request, CancellationToken cancellationToken)
        {
            var quiz = await quizService.GetQuizByIdAsync(request.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(request.Id);
            if (quizQuestion == null)
                return NotFound<string>();

            mapper.Map(request, quizQuestion);
            var result = await quizQuestionService.UpdateQuestionAsync(quizQuestion);

            return result != null
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Updated])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
        }

        public async Task<Response<string>> Handle(DeleteQuizQuestionsCommand request, CancellationToken cancellationToken)
        {
            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(request.Id);
            if (quizQuestion == null)
                return NotFound<string>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            var result = await quizQuestionService.DeleteQuestionAsync(request.Id);

            return result != null
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
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

using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.QuestionOptions.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.QuestionOptions.Command.Handler
{
    public class QuestionOptionsCommandHandler : ResponseHandler,
        IRequestHandler<AddQuestionOptionsCommand, Response<string>>,
        IRequestHandler<DeleteQuestionOptionsCommand, Response<string>>,
        IRequestHandler<UpdateQuestionOptionsCommand, Response<string>>

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

        public QuestionOptionsCommandHandler(IQuestionOptionsService questionOptionsService, IMapper mapper, IQuizQuestionService quizQuestionService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, UserManager<Users> userManager, RoleManager<Role> roleManager, IUserCoursesService userCoursesService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService, IQuizService quizService) : base(stringLocalizer)
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


        public async Task<Response<string>> Handle(AddQuestionOptionsCommand request, CancellationToken cancellationToken)
        {
            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(request.QuizQuestionId);
            if (quizQuestion == null)
                return NotFound<string>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            var option = mapper.Map<LMS.Data_.Entities.Quiz.QuestionOptions>(request);
            var result = await questionOptionsService.AddOptionAsync(option);

            return result != null
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Created])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
        }

        public async Task<Response<string>> Handle(DeleteQuestionOptionsCommand request, CancellationToken cancellationToken)
        {
            var option = await questionOptionsService.GetOptionByIdAsync(request.Id);
            if (option == null)
                return NotFound<string>();

            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(option.QuizQuestionId);
            if (quizQuestion == null)
                return NotFound<string>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            var result = await questionOptionsService.DeleteOptionAsync(option.Id);

            return result == true
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Deleted])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
        }

        public async Task<Response<string>> Handle(UpdateQuestionOptionsCommand request, CancellationToken cancellationToken)
        {
            var option = await questionOptionsService.GetOptionByIdAsync(request.Id);
            if (option == null)
                return NotFound<string>();

            var quizQuestion = await quizQuestionService.GetQuestionByIdAsync(option.QuizQuestionId);
            if (quizQuestion == null)
                return NotFound<string>();

            var quiz = await quizService.GetQuizByIdAsync(quizQuestion.QuizId);
            if (quiz == null)
                return NotFound<string>();

            var authResult = await AuthorizeCourseAccessAsync<string>(quiz.CourseId);
            if (authResult != null) return authResult;

            mapper.Map(request, option);
            var result = await questionOptionsService.UpdateOptionAsync(option);

            return result != null
                ? Success<string>(stringLocalizer[SharedResourcesKeys.Updated])
                : BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
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

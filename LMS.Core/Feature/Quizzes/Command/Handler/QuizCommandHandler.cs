using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Quizzes.Command.Model;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Service.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Quizzes.Command.Handler
{
    public class QuizCommandHandler : ResponseHandler,
        IRequestHandler<AddQuizCommand, Response<string>>,
        IRequestHandler<DeleteQuizCommand, Response<string>>
        , IRequestHandler<UpdateQuizCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IQuizService quizService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ICoursesService coursesService;

        public QuizCommandHandler(IMapper mapper, IQuizService quizService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, IInstructorProfilesService instructorProfilesService, ICoursesService coursesService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.quizService = quizService;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
            this.instructorProfilesService = instructorProfilesService;
            this.coursesService = coursesService;
        }

        public async Task<Response<string>> Handle(AddQuizCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
            if (instructorProfile == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            var course = await coursesService.GetCourseById(request.CourseId);
            if (course == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);

            if (course.InstructorProfileId != instructorProfile.Id)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UnAuthorized]);

            var quiz = mapper.Map<LMS.Data_.Entities.Quiz.Quizzes>(request);
            var result = await quizService.AddQuizAsync(quiz);

            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);

            return Success<string>(stringLocalizer[SharedResourcesKeys.Created]);
        }

        public async Task<Response<string>> Handle(DeleteQuizCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return (BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]));
            var quiz = await quizService.GetQuizByIdAsync(request.Id);
            if (quiz == null)
                return (NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]));

            var result = await quizService.DeleteQuizAsync(request.Id);
            if (!result)
                return (BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]));
            return (Success<string>(stringLocalizer[SharedResourcesKeys.Deleted]));



        }

        public async Task<Response<string>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return (BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]));
            var quiz = await quizService.GetQuizByIdAsync(request.Id);
            if (quiz == null)
                return (NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]));

            var userId = currentUserService.UserId;
            var instructorProfile = await instructorProfilesService.GetInstructorProfilesByUserId(userId);
            if (instructorProfile == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.NotFound]);
            if (quiz.Course.InstructorProfileId != instructorProfile.Id)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UnAuthorized]);
            mapper.Map(request, quiz);
            quiz.UpdatedAt = DateTime.UtcNow;
            await quizService.UpdateQuizAsync(quiz);
            return Success<string>(stringLocalizer[SharedResourcesKeys.Updated]);
        }
    }
}

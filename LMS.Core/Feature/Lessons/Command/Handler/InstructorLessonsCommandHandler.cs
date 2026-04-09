using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Lessons.Command.Models;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
namespace LMS.Core.Feature.Lessons.Command.Handler
{
    public class InstructorLessonsCommandHandler : ResponseHandler,
        IRequestHandler<AddLessonCommand, Response<string>>
        , IRequestHandler<UpdateLessonCommand, Response<string>>
        , IRequestHandler<DeleteLessonCommand, Response<string>>
        , IRequestHandler<ReorderLessonCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly ILessonsService lessonsService;
        private readonly UserManager<Users> userManager;
        private readonly ICoursesService coursesService;
        private readonly IMediator mediator;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IFileService fileService;

        public InstructorLessonsCommandHandler(IMapper mapper,
            ILessonsService lessonsService,
            UserManager<Users> userManager,
            ICoursesService coursesService,
            IMediator mediator, IStringLocalizer<SharedResources> stringLocalizer, IFileService fileService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.lessonsService = lessonsService;
            this.userManager = userManager;
            this.coursesService = coursesService;
            this.mediator = mediator;
            this.stringLocalizer = stringLocalizer;
            this.fileService = fileService;
        }

        public async Task<Response<string>> Handle(AddLessonCommand request, CancellationToken cancellationToken)
        {
            var CourseIsFound = await coursesService.GetCourseById(request.CourseId);
            if (CourseIsFound == null) return NotFound<string>();
            if (CourseIsFound.CourseStatus != Data_.Enum.CourseStatusEnum.Approved)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Coursenotapproved]);
            var LessonMapped = mapper.Map<LMS.Data_.Entities.Lessons>(request);
            LessonMapped.CreatedAt = DateTime.UtcNow;
            if (await lessonsService.NumberOfOrderIsFound(request.CourseId, request.OrderNumber))
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.ordernumberisalreadyused]);
            var Result = await lessonsService.Add(LessonMapped, request.Video);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);

            // Notify To Increase  Number Of Lessons In Course Table 
            await mediator.Publish(new IncreaseNumberOfLessonsAndDurationHoursInCourse(request.CourseId, request.DurationMinutes));

            return Created<string>(stringLocalizer[SharedResourcesKeys.LessonAdded]);
        }
        public async Task<Response<string>> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {

            var course = await coursesService.GetCourseById(request.CourseId);
            if (course == null)
                return NotFound<string>();

            if (course.CourseStatus != Data_.Enum.CourseStatusEnum.Approved)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Coursenotapproved]);


            if (await lessonsService.OrderNumberExistsAsync(request.CourseId, request.OrderNumber, request.Id))
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.ordernumberisalreadyused]);

            var lessonEntity = mapper.Map<LMS.Data_.Entities.Lessons>(request);

            var updatedLesson = await lessonsService.Update(lessonEntity, request.Video);
            if (updatedLesson == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            return Updated<string>(stringLocalizer[SharedResourcesKeys.LessonUpdated]);
        }

        public async Task<Response<string>> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            if (request.LessonId <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var LessonsIsFound = await lessonsService.GetLessonsById(request.LessonId);
            if (LessonsIsFound == null) return NotFound<string>();

            var Result = await lessonsService.DeleteLesson(request.LessonId);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            await mediator.Publish(new DecreaseNumberOfAndDurationHoursLessonsInCourse(LessonsIsFound.CourseId, LessonsIsFound.Course.DurationHours));
            return Deleted<string>(stringLocalizer[SharedResourcesKeys.LessonDeleted]);

        }

        public async Task<Response<string>> Handle(ReorderLessonCommand request, CancellationToken cancellationToken)
        {
            var lesson = await lessonsService.GetLessonsById(request.lessonId);

            if (lesson is null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.LessonNotFound]);

            if (lesson.CourseId != request.courseId)
                return NotFound<string>("Lesson does not belong to this course");

            var orderExists = await lessonsService.OrderNumberExistsAsync(
                request.courseId,
                request.orderNumber,
                request.lessonId);

            if (orderExists)
                return BadRequest<string>($"Order number {request.orderNumber} is already taken");

            lesson.OrderNumber = request.orderNumber;

            var Update = await lessonsService.Update(lesson, null);
            if (Update is null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            return Updated<string>("Lesson reordered successfully");
        }
    }
}

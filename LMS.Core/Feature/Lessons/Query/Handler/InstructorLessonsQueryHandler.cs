using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Lessons.Query.Models;
using LMS.Core.Feature.Lessons.Query.Result;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Lessons.Query.Handler
{
    public class InstructorLessonsQueryHandler : ResponseHandler,
        IRequestHandler<GetLessonsByCourseIdQuery, Response<List<GetAllLessonsByCourseIdResult>>>,
        IRequestHandler<GetLessonByIdQuery, Response<GetLessonByIdResult>>
    {
        private readonly IMapper mapper;
        private readonly ILessonsService lessonsService;
        private readonly ICoursesService coursesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public InstructorLessonsQueryHandler(IMapper mapper, ILessonsService lessonsService, ICoursesService coursesService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.lessonsService = lessonsService;
            this.coursesService = coursesService;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<List<GetAllLessonsByCourseIdResult>>> Handle(GetLessonsByCourseIdQuery request, CancellationToken cancellationToken)
        {
            var Course = await coursesService.GetCourseById(request.CourseId);
            if (Course == null) return NotFound<List<GetAllLessonsByCourseIdResult>>();

            var Lessons = await lessonsService.GetAllLessonsByCourseId(request.CourseId);
            if (Lessons.Count <= 0)
                return NotFound<List<GetAllLessonsByCourseIdResult>>(stringLocalizer[SharedResourcesKeys.CourseDonotHaveAnyLessons]);
            var LessonsMapped = mapper.Map<List<GetAllLessonsByCourseIdResult>>(Lessons);
            var CountOfLessons = await lessonsService.GetCountOfLessonsByCourseId(request.CourseId);
            return Success(LessonsMapped, $"Count Of Lessons = {CountOfLessons}");
        }

        public async Task<Response<GetLessonByIdResult>> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0) return BadRequest<GetLessonByIdResult>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Lesson = await lessonsService.GetLessonsById(request.Id);
            if (Lesson == null) return NotFound<GetLessonByIdResult>();
            var Mapped = mapper.Map<GetLessonByIdResult>(Lesson);
            return Success(Mapped);




        }
    }
}

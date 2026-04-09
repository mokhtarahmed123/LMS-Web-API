using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Courses.Query.Models;
using LMS.Core.Feature.Courses.Query.Models.AdminModel;
using LMS.Core.Feature.Courses.Query.Result;
using LMS.Core.Feature.Courses.Query.Result.AdminResultQuery;
using LMS.Core.Resources;
using LMS.Core.Wrappers;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LMS.Core.Feature.Courses.Query.Handler
{
    public class AdminCourseQueryHandler : ResponseHandler,
        IRequestHandler<GetAllCoursesQuery, Response<List<GetAllCoursesResult>>>,
        IRequestHandler<GetCourseByIdQuery, Response<GetCourseByIdResult>>
        , IRequestHandler<GetAllCoursesByCategoryIdQuery, Response<List<GetAllCoursesByCategoryIdResult>>>
        , IRequestHandler<GetAllCoursesByInstructorIdQuery, Response<List<GetAllCoursesByInstructorIdResult>>>
        , IRequestHandler<GetAllCoursesPendingQuery, Response<List<GetAllCoursesPendingResult>>>
        , IRequestHandler<GetAllCoursesPaginatedQuery, PaginatedResult<GetAllCoursesPaginatedResult>>

    {
        private readonly IMapper mapper;
        private readonly ICoursesService coursesService;
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminCourseQueryHandler(IMapper mapper, ICoursesService coursesService, ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.coursesService = coursesService;
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<List<GetAllCoursesResult>>> Handle(
            GetAllCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var listOfCourses = await coursesService.GetAllCoursesAsync();

            if (listOfCourses == null || !listOfCourses.Any())
                return NotFound<List<GetAllCoursesResult>>();

            var mappedCourses = mapper.Map<List<GetAllCoursesResult>>(listOfCourses);

            return Success(mappedCourses);
        }

        public async Task<Response<GetCourseByIdResult>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.CourseId <= 0)
                return BadRequest<GetCourseByIdResult>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var CourseIsFound = await coursesService.GetCourseById(request.CourseId);
            if (CourseIsFound == null)
                return NotFound<GetCourseByIdResult>();
            var mappedCourse = mapper.Map<GetCourseByIdResult>(CourseIsFound);
            return Success(mappedCourse);

        }

        public async Task<Response<List<GetAllCoursesByCategoryIdResult>>> Handle(GetAllCoursesByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var CategoryIsFound = await categoriesService.GetCategoryById(request.CategoryId);
            if (CategoryIsFound == null)
                return NotFound<List<GetAllCoursesByCategoryIdResult>>();
            var listOfCourses = await coursesService.GetCoursesByCategoryIdAsync(request.CategoryId);
            if (listOfCourses == null || !listOfCourses.Any())
                return NotFound<List<GetAllCoursesByCategoryIdResult>>(stringLocalizer[SharedResourcesKeys.NocoursesfoundforThiscategory]);
            var mappedCourses = mapper.Map<List<GetAllCoursesByCategoryIdResult>>(listOfCourses);
            return Success(mappedCourses);

        }

        public async Task<Response<List<GetAllCoursesByInstructorIdResult>>> Handle(GetAllCoursesByInstructorIdQuery request, CancellationToken cancellationToken)
        {
            if (request.InstructorId <= 0)
                return BadRequest<List<GetAllCoursesByInstructorIdResult>>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var listOfCourses = await coursesService.GetCoursesByInstructorIdAsync(request.InstructorId);
            if (listOfCourses == null || !listOfCourses.Any())
                return (NotFound<List<GetAllCoursesByInstructorIdResult>>(stringLocalizer[SharedResourcesKeys.NocoursesfoundforThisInstructor]));
            var mappedCourses = mapper.Map<List<GetAllCoursesByInstructorIdResult>>(listOfCourses);

            return Success(mappedCourses);

        }

        public async Task<Response<List<GetAllCoursesPendingResult>>> Handle(GetAllCoursesPendingQuery request, CancellationToken cancellationToken)
        {
            var List = await coursesService.GetAllCoursesPending();
            if (List == null || !List.Any())
                return NotFound<List<GetAllCoursesPendingResult>>();
            var mappedCourses = mapper.Map<List<GetAllCoursesPendingResult>>(List);

            return Success(mappedCourses);


        }

        public Task<PaginatedResult<GetAllCoursesPaginatedResult>> Handle(GetAllCoursesPaginatedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<LMS.Data_.Entities.Courses, GetAllCoursesPaginatedResult>> expression =
                e => new GetAllCoursesPaginatedResult(e.Title, e.Description, e.InstructorProfile.User.UserName,
                e.InstructorProfile.User.Email, e.DurationHours, e.Level,
                e.CourseLanguage, e.ThumbnailUrl, e.NumberOfEnrolledStudents, e.NumberOfLessons,
                e.AverageRating, e.Category.Name, e.CourseStatus);
            var Quarable = coursesService.GetAllCoursesPaginated();
            var PaginatedResult = Quarable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return PaginatedResult;




        }
    }
}

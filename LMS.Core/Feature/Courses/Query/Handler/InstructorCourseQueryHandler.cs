using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Courses.Query.Models.InstructorModel;
using LMS.Core.Feature.Courses.Query.Result.InstructorResultQuery;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Courses.Query.Handler
{
    internal class InstructorCourseQueryHandler : ResponseHandler,
        IRequestHandler<GetMyCoursesQuery, Response<List<GetMyCoursesResult>>>

    {
        private readonly IMapper mapper;
        private readonly ICoursesService coursesService;
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IUserCoursesService userCoursesService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ILessonsService lessonsService;
        private readonly UserManager<Users> userManager;

        public InstructorCourseQueryHandler(IMapper mapper, ICoursesService coursesService, ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer, IUserCoursesService userCoursesService, IHttpContextAccessor httpContextAccessor, IInstructorProfilesService instructorProfilesService, ILessonsService lessonsService, UserManager<Users> userManager) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.coursesService = coursesService;
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
            this.userCoursesService = userCoursesService;
            this.httpContextAccessor = httpContextAccessor;
            this.instructorProfilesService = instructorProfilesService;
            this.lessonsService = lessonsService;
            this.userManager = userManager;
        }

        public async Task<Response<List<GetMyCoursesResult>>> Handle(GetMyCoursesQuery request, CancellationToken cancellationToken)
        {
            var UserId = GetUserId();
            var Instructor = await instructorProfilesService.GetInstructorByUserId(UserId);
            if (Instructor == null) return NotFound<List<GetMyCoursesResult>>();
            if (Instructor.StatusOfInstructorProfile != Data_.Enum.StatusOfInstructorProfileEnum.Approved)
                return BadRequest<List<GetMyCoursesResult>>("Request Not Approved Yet!");

            var Courses = await coursesService.GetCoursesByInstructorIdAsync(Instructor.Id);
            if (!Courses.Any()) return NotFound<List<GetMyCoursesResult>>("You Don't Own Any Courses, Go To Add Course");

            var Mapped = mapper.Map<List<GetMyCoursesResult>>(Courses);

            foreach (var course in Mapped)
            {
                List<string> StudentsIds = await userCoursesService.GetUsersIdsByCourseId(course.Id);

                var studentsResults = new List<StudentsResult>();
                foreach (var studentId in StudentsIds)
                {
                    var Student = await userManager.FindByIdAsync(studentId);
                    if (Student == null) continue;

                    studentsResults.Add(new StudentsResult
                    {
                        StudentEmail = Student.Email,
                        StudentName = Student.UserName
                    });
                }

                course.Students = studentsResults;
            }

            return Success(Mapped);
        }



        private string GetUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var claim = user?.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(claim))
                throw new Exception("User Id claim not found");

            return claim;
        }
    }
}

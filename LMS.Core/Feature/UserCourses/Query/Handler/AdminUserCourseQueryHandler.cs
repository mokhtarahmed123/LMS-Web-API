using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.UserCourses.Query.Models.AdminModel;
using LMS.Core.Feature.UserCourses.Query.Result;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.UserCourses.Query.Handler
{
    internal class AdminUserCourseQueryHandler : ResponseHandler,
        IRequestHandler<GetAllStudentByCourseIdQuery, Response<List<GetAllStudentByCourseIdResult>>>

    {
        private readonly IMapper mapper;
        private readonly IUserCoursesService userCoursesService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        private readonly ICoursesService coursesService;
        private readonly IInstructorProfilesService instructorProfilesService;

        public AdminUserCourseQueryHandler(IMapper mapper, IUserCoursesService userCoursesService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, ICoursesService coursesService, IInstructorProfilesService instructorProfilesService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userCoursesService = userCoursesService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.coursesService = coursesService;
            this.instructorProfilesService = instructorProfilesService;
        }
        public async Task<Response<List<GetAllStudentByCourseIdResult>>> Handle(GetAllStudentByCourseIdQuery request, CancellationToken cancellationToken)
        {
            if (request.CourseId <= 0) return BadRequest<List<GetAllStudentByCourseIdResult>>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var CourseIsFound = await coursesService.GetCourseById(request.CourseId);
            if (CourseIsFound == null) return NotFound<List<GetAllStudentByCourseIdResult>>();
            var CoursesUser = await userCoursesService.GetUsersCourseIdsByCourseId(request.CourseId);
            if (!CoursesUser.Any()) return NotFound<List<GetAllStudentByCourseIdResult>>();
            var Mapped = mapper.Map<List<GetAllStudentByCourseIdResult>>(CoursesUser);
            var Count = await userCoursesService.GetCountOfStudentByCourseId(request.CourseId);
            return Success(Mapped, $" Total  Students ={Count}");

        }

    }
}

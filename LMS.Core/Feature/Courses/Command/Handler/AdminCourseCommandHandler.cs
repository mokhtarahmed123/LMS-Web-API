using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Courses.Command.Models.AdminCommand;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Courses.Command.Handler
{
    internal class AdminCourseCommandHandler : ResponseHandler,
        IRequestHandler<DeleteCourseByAdminCommand, Response<string>>,
        IRequestHandler<ApproveCourseCommand, Response<string>>,
        IRequestHandler<RejectCourseCommand, Response<string>>
    {
        private readonly ICoursesService coursesService;
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly ICategoriesService categoriesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminCourseCommandHandler(ICoursesService coursesService, IMapper mapper, UserManager<Users> userManager, IInstructorProfilesService instructorProfilesService, ICategoriesService categoriesService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.coursesService = coursesService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.instructorProfilesService = instructorProfilesService;
            this.categoriesService = categoriesService;
            this.stringLocalizer = stringLocalizer;
        }

        //public async Task<Response<string>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        //{
        //    if (request.CourseId <= 0)
        //        return BadRequest<string>("Invalid Course  ID.");
        //    var Course = await coursesService.GetCourseById(request.CourseId);
        //    if (Course == null)
        //        return NotFound<string>($"Course With Id {request.CourseId} Not Found");
        //    var Result = await coursesService.DeleteCourse(request.CourseId);
        //    if (!Result) return BadRequest<string>("Failed Delete Course");
        //    return Deleted<string>("Deleted Successfully");

        //}
        public async Task<Response<string>> Handle(RejectCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Course = await coursesService.GetCourseById(request.CourseId);
            if (Course == null)
                return (NotFound<string>());
            Course.CourseStatus = CourseStatusEnum.Rejected;
            Course.UpdatedAt = DateTime.Now;
            Course.ReasonOfReject = request.Reasons;
            var Result = await coursesService.UpdateCourse(Course, null);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>();
        }

        public async Task<Response<string>> Handle(ApproveCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Course = await coursesService.GetCourseById(request.CourseId);
            if (Course == null)
                return (NotFound<string>());
            Course.CourseStatus = CourseStatusEnum.Approved;
            Course.UpdatedAt = DateTime.Now;
            var Result = await coursesService.UpdateCourse(Course, null);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>();

        }

        public async Task<Response<string>> Handle(DeleteCourseByAdminCommand request, CancellationToken cancellationToken)
        {
            if (request.CourseId <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Course = await coursesService.GetCourseById(request.CourseId);
            if (Course == null)
                return NotFound<string>();
            var Result = await coursesService.DeleteCourse(request.CourseId);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>();

        }
    }
}

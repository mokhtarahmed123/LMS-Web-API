using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Courses.Command.Models.InstructorCommand;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Courses.Command.Handler
{
    public class InstructorCourseCommandHandler : ResponseHandler,
        IRequestHandler<AddCourseCommand, Response<string>>,
        IRequestHandler<DeleteCourseByInstructorCommand, Response<string>>,
        IRequestHandler<UpdateCourseCommand, Response<string>>



    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly ICoursesService coursesService;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService common;

        public InstructorCourseCommandHandler(IMapper mapper, UserManager<Users> userManager, ICoursesService coursesService, IInstructorProfilesService instructorProfilesService, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService common) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.coursesService = coursesService;
            this.instructorProfilesService = instructorProfilesService;
            this.stringLocalizer = stringLocalizer;
            this.common = common;
        }
        public async Task<Response<string>> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var InstructorIdAsUserId = common.UserIdFromJWT();
            var InstructorISfound = await instructorProfilesService.GetInstructorProfilesByUserId(InstructorIdAsUserId);

            var instructor = await instructorProfilesService
                .GetById(InstructorISfound.Id);

            if (instructor == null)
                return NotFound<string>();

            if (instructor.StatusOfInstructorProfile != StatusOfInstructorProfileEnum.Approved)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.Instructorprofileisnotapproved]);



            var course = mapper.Map<LMS.Data_.Entities.Courses>(request);

            course.InstructorProfileId = instructor.Id;
            course.CourseStatus = CourseStatusEnum.Pending;
            course.NumberOfLessons = 0;
            course.AverageRating = 0;
            course.NumberOfEnrolledStudents = 0;
            course.DurationHours = 1;
            course.CreatedAt = DateTime.UtcNow;

            var result = await coursesService.AddCourse(course, request.ProfilePicture);

            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);

            return Created<string>(null);
        }

        public async Task<Response<string>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var InstructorIdAsUserId = common.UserIdFromJWT();
            var InstructorISfound = await instructorProfilesService.GetInstructorProfilesByUserId(InstructorIdAsUserId);

            var instructor = await instructorProfilesService.GetById(InstructorISfound.Id);
            if (instructor == null)
                return NotFound<string>();


            var existingCourse = await coursesService.GetCourseById(request.CourseId);
            if (existingCourse == null || existingCourse.InstructorProfileId != InstructorISfound.Id)
                return NotFound<string>(" Course Not Found Or You Are Not Owner This Course ");


            mapper.Map(request, existingCourse);

            if (request.ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "images", "Course");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ProfilePicture.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.ProfilePicture.CopyToAsync(stream, cancellationToken);

                existingCourse.ThumbnailUrl = $"/images/courses/{fileName}";
            }

            existingCourse.UpdatedAt = DateTime.Now;
            var result = await coursesService.UpdateCourse(existingCourse, null);
            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            return Updated<string>();
        }
        public async Task<Response<string>> Handle(DeleteCourseByInstructorCommand request, CancellationToken cancellationToken)
        {
            var InstructorId = common.UserIdFromJWT();
            var InstructorISfound = await instructorProfilesService.GetInstructorProfilesByUserId(InstructorId);

            if (request.CourseId <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);

            var existingCourse = await coursesService.GetCourseById(request.CourseId);
            if (existingCourse == null || existingCourse.InstructorProfileId != InstructorISfound.Id)
                return NotFound<string>(" Course Not Found Or You Are Not Owner This Course ");
            var Result = await coursesService.DeleteCourse(request.CourseId);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>();
        }


        //private string GetUserId()
        //{
        //    var user = httpContextAccessor.HttpContext?.User;
        //    var claim = user?.FindFirst("Id")?.Value;

        //    if (string.IsNullOrEmpty(claim))
        //        throw new Exception("User Id claim not found");

        //    return claim;
        //}

    }
}

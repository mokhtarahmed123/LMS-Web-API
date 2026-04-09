using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.UserCourses.Command.Models.Student;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
namespace LMS.Core.Feature.UserCourses.Command.Handler
{
    public class StudentUserCourseCommandHandler :
        ResponseHandler,
        IRequestHandler<EnrollUserCourseCommand, Response<string>>,
        IRequestHandler<UnenrollCourseCommand, Response<string>>
        , IRequestHandler<RateUserCourseCommand, Response<string>>
        , IRequestHandler<favouriteUserCourseCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly IUserCoursesService userCoursesService;
        private readonly ICoursesService coursesService;
        private readonly IMediator mediator;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ICurrentUserService common;

        public StudentUserCourseCommandHandler(IMapper mapper, UserManager<Users> userManager, IUserCoursesService userCoursesService, ICoursesService coursesService, IMediator mediator, IStringLocalizer<SharedResources> stringLocalizer, IHttpContextAccessor httpContextAccessor, ICurrentUserService common) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.userCoursesService = userCoursesService;
            this.coursesService = coursesService;
            this.mediator = mediator;
            this.stringLocalizer = stringLocalizer;
            this.httpContextAccessor = httpContextAccessor;
            this.common = common;
        }
        public async Task<Response<string>> Handle(EnrollUserCourseCommand request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT();  // GetUserId();
            var UserIsFound = await userManager.FindByIdAsync(UserId);
            if (UserIsFound == null) return NotFound<string>((stringLocalizer[SharedResourcesKeys.UserNotFound]));
            var CourseIsFound = await coursesService.GetCourseById(request.CourseId);
            if (CourseIsFound == null)
            {
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.CourseNotFound]);
            }
            var Result = mapper.Map<LMS.Data_.Entities.UserCourses>(request);
            Result.EnrolledAt = DateTime.UtcNow;
            Result.Rating = 0;
            Result.IsFavorite = false;
            Result.UserId = UserId;
            var UserCourse = await userCoursesService.Add(Result);
            if (UserCourse != null)
            {
                // Event To Notify Adding Student That Enroll Courses Done { ++ }  
                await mediator.Publish(new IncreaseCourseEnrollmentNotification(request.CourseId));

                return Created<string>(null);

            }
            else
            {
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
            }
        }
        public async Task<Response<string>> Handle(UnenrollCourseCommand request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT();  //GetUserId();

            var UserIsFoundInCourse = await userCoursesService.IsUserEnrolledInCourseAsync(UserId, request.CourseId);
            if (UserIsFoundInCourse == null) return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotEnrolledInThisCourse]);
            var Result = mapper.Map<LMS.Data_.Entities.UserCourses>(request);
            var UserCourse = await userCoursesService.Delete(UserId, request.CourseId);
            if (!UserCourse)
            {

                // Event To Notify Adding Student That Enroll Courses Done { ++ }  
                await mediator.Publish(new DecreaseCourseEnrollmentNotification(request.CourseId));
                return Deleted<string>();

            }
            else
            {
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            }

        }
        public async Task<Response<string>> Handle(RateUserCourseCommand request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT();//GetUserId();

            var userCourse = await userCoursesService
                .GetUserCourseAsync(UserId, request.CourseId);

            if (userCourse == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotEnrolledInThisCourse]);

            userCourse.Rating = request.Rating;

            var result = userCoursesService.Update(userCourse);

            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            await mediator.Publish(new RateCourseEnrollmentNotification(request.CourseId, request.Rating));

            return Updated<string>();
        }
        public async Task<Response<string>> Handle(favouriteUserCourseCommand request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT();//GetUserId();

            var userCourse = await userCoursesService
                .GetUserCourseAsync(UserId, request.CourseId);

            if (userCourse == null)
                return NotFound<string>(stringLocalizer[SharedResourcesKeys.UserNotEnrolledInThisCourse]);

            userCourse.IsFavorite = request.IsFavourite;

            var result = userCoursesService.Update(userCourse);

            if (result == null)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);

            return Updated<string>();
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
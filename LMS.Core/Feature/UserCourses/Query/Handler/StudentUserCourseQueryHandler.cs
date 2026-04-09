using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.UserCourses.Query.Models.StudentModel;
using LMS.Core.Feature.UserCourses.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.UserCourses.Query.Handler
{
    public class StudentUserCourseQueryHandler : ResponseHandler,
        IRequestHandler<GetAllMyCoursesEnrollmentsQuery, Response<List<GetAllMyCoursesEnrollmentsResult>>>
        , IRequestHandler<GetMyFavouriteCoursesByUserIdQuery, Response<List<GetMyFavouriteCoursesByUserIdResult>>>
    {
        private readonly IMapper mapper;
        private readonly IUserCoursesService userCoursesService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        private readonly ICurrentUserService common;

        public StudentUserCourseQueryHandler(IMapper mapper, IUserCoursesService userCoursesService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService common) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userCoursesService = userCoursesService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.common = common;
        }


        public async Task<Response<List<GetAllMyCoursesEnrollmentsResult>>> Handle(GetAllMyCoursesEnrollmentsQuery request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT(); // GetUserId();
            var UserIsFound = await userManager.FindByIdAsync(UserId);
            if (UserIsFound == null) return (NotFound<List<GetAllMyCoursesEnrollmentsResult>>(stringLocalizer[SharedResourcesKeys.UserNotFound]));
            var UserCourses = await userCoursesService.GetUserCoursesByUserIdAsync(UserId);
            if (UserCourses == null || UserCourses.Count == 0)
            {
                return (NotFound<List<GetAllMyCoursesEnrollmentsResult>>());
            }
            var Result = mapper.Map<List<GetAllMyCoursesEnrollmentsResult>>(UserCourses);
            return (Success<List<GetAllMyCoursesEnrollmentsResult>>(Result));


        }

        public async Task<Response<List<GetMyFavouriteCoursesByUserIdResult>>> Handle(GetMyFavouriteCoursesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var UserId = common.UserIdFromJWT(); // GetUserId();
            var UserIsFound = await userManager.FindByIdAsync(UserId);
            if (UserIsFound == null) return (NotFound<List<GetMyFavouriteCoursesByUserIdResult>>(stringLocalizer[SharedResourcesKeys.UserNotFound]));
            var UserCourses = await userCoursesService.GetUserCoursesByUserIdAsync(UserId);
            if (UserCourses == null || UserCourses.Count == 0)
            {
                return (NotFound<List<GetMyFavouriteCoursesByUserIdResult>>(stringLocalizer[SharedResourcesKeys.NoCoursesFound]));
            }
            if (!UserCourses.Any(uc => uc.IsFavorite))
            {
                return (NotFound<List<GetMyFavouriteCoursesByUserIdResult>>());
            }
            var Result = mapper.Map<List<GetMyFavouriteCoursesByUserIdResult>>(UserCourses);
            return (Success<List<GetMyFavouriteCoursesByUserIdResult>>(Result));

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

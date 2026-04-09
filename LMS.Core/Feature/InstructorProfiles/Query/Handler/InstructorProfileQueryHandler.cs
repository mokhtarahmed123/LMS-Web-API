using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.InstructorProfiles.Query.Models;
using LMS.Core.Feature.InstructorProfiles.Query.Result;
using LMS.Core.Resources;
using LMS.Core.Wrappers;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LMS.Core.Feature.InstructorProfiles.Query.Handler
{
    public class InstructorProfileQueryHandler : ResponseHandler,
        IRequestHandler<GetAllInstructorProfilesQuery, Response<List<GetAllInstructorProfilesResult>>>,
        IRequestHandler<GetByIdInstructorProfileQuery, Response<GetByIdInstructorProfileResult>>
        , IRequestHandler<GetInstructorByUserIdProfileQuery, Response<GetInstructorByUserIdProfileResult>>,
         IRequestHandler<GetAlInstructorProfilesPaginatedQuery, PaginatedResult<GetAllInstructorProfilesPaginatedResult>>
        , IRequestHandler<GetAllInstructorProfilesByFilterQuery, Response<List<GetAllInstructorProfilesByFilterResult>>>
        , IRequestHandler<GetMyRequestInstructorProfileQuery, Response<GetMyRequestInstructorProfileResult>>
              , IRequestHandler<MyStatsAsInstructorQuery, Response<MyStatsAsInstructorResult>>
    {
        private readonly IMapper mapper;
        private readonly IInstructorProfilesService instructorProfilesService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;
        private readonly ICoursesService coursesService;
        private readonly IUserCoursesService userCoursesService;

        public InstructorProfileQueryHandler(IMapper mapper, IInstructorProfilesService instructorProfilesService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService, ICoursesService coursesService, IUserCoursesService userCoursesService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.instructorProfilesService = instructorProfilesService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
            this.coursesService = coursesService;
            this.userCoursesService = userCoursesService;
        }
        public async Task<Response<List<GetAllInstructorProfilesResult>>> Handle(GetAllInstructorProfilesQuery request, CancellationToken cancellationToken)
        {
            var instructorProfiles = await instructorProfilesService.GetAll();
            if (instructorProfiles == null || instructorProfiles.Count == 0)
                return NotFound<List<GetAllInstructorProfilesResult>>();
            var result = new List<GetAllInstructorProfilesResult>();
            foreach (var item in instructorProfiles)
            {
                var user = await userManager.FindByIdAsync(item.UserId);
                var mapped = mapper.Map<GetAllInstructorProfilesResult>(item);
                mapped.Name = user.UserName;
                result.Add(mapped);
            }
            return Success(result);


        }

        public async Task<Response<GetByIdInstructorProfileResult>> Handle(GetByIdInstructorProfileQuery request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return BadRequest<GetByIdInstructorProfileResult>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var instructorProfile = await instructorProfilesService.GetById(request.Id);
            if (instructorProfile == null)
                return NotFound<GetByIdInstructorProfileResult>();
            var result = mapper.Map<GetByIdInstructorProfileResult>(instructorProfile);
            var user = await userManager.FindByIdAsync(instructorProfile.UserId);
            result.Name = user.UserName;
            result.Email = user.Email;
            return Success(result);


        }

        public async Task<Response<GetInstructorByUserIdProfileResult>> Handle(GetInstructorByUserIdProfileQuery request, CancellationToken cancellationToken)
        {
            var instructorProfile =
          await instructorProfilesService
              .GetInstructorProfilesByUserId(request.UserId);

            if (instructorProfile == null)
                return NotFound<GetInstructorByUserIdProfileResult>();

            var result = mapper.Map<GetInstructorByUserIdProfileResult>(instructorProfile);

            return Success(result);
        }

        public async Task<PaginatedResult<GetAllInstructorProfilesPaginatedResult>> Handle(GetAlInstructorProfilesPaginatedQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<LMS.Data_.Entities.InstructorProfiles, GetAllInstructorProfilesPaginatedResult>> expression = e => new GetAllInstructorProfilesPaginatedResult(e.Id, e.User.UserName, e.Bio, e.ProfilePictureUrl, e.LinkedInUrl, e.StatusOfInstructorProfile.ToString(), DateOnly.FromDateTime(e.CreatedAt), e.ReasonOfReject);
            var quarable = instructorProfilesService.GetAllInstructorsAsQueryable();
            var PaginatedList = await quarable.Select(expression).ToPaginatedListAsync((int)request.PageNumber, (int)request.PageSize);
            return PaginatedList;

        }

        public async Task<Response<List<GetAllInstructorProfilesByFilterResult>>> Handle(GetAllInstructorProfilesByFilterQuery request, CancellationToken cancellationToken)
        {
            var instructorProfiles = await instructorProfilesService.GetAllInstructorsFilter(request.statusOfInstructorProfileEnum);
            var mappedResult = mapper.Map<List<GetAllInstructorProfilesByFilterResult>>(instructorProfiles);
            return Success(mappedResult);
        }

        public async Task<Response<GetMyRequestInstructorProfileResult>> Handle(GetMyRequestInstructorProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var Userisfound = userManager.Users.Any(u => u.Id == userId);
            if (!Userisfound)
                return (NotFound<GetMyRequestInstructorProfileResult>(stringLocalizer[SharedResourcesKeys.UserNotFound]));
            var instructorProfile = await instructorProfilesService.GetMyRequest(userId);
            if (instructorProfile == null)
                return NotFound<GetMyRequestInstructorProfileResult>(stringLocalizer[SharedResourcesKeys.YouDonthaveanyrequest]);
            var result = mapper.Map<GetMyRequestInstructorProfileResult>(instructorProfile);
            return Success(result);



        }

        public async Task<Response<MyStatsAsInstructorResult>> Handle(MyStatsAsInstructorQuery request, CancellationToken cancellationToken)
        {
            var Id = currentUserService.UserId;
            var Instructor = await instructorProfilesService.GetInstructorByUserId(Id);
            if (Instructor == null) return NotFound<MyStatsAsInstructorResult>();
            if (Instructor.StatusOfInstructorProfile != Data_.Enum.StatusOfInstructorProfileEnum.Approved)
                return BadRequest<MyStatsAsInstructorResult>("Request Not Approved Yet!");

            MyStatsAsInstructorResult myStatsAsInstructorResult = new MyStatsAsInstructorResult();
            var courses = await coursesService.GetCoursesByInstructorIdAsync(Instructor.Id);
            var ListOfUsersCourse = await userCoursesService
        .GetAll(Instructor.Id);

            myStatsAsInstructorResult.Id = Id;

            myStatsAsInstructorResult.approvedCourses = courses.Count(c => c.CourseStatus == CourseStatusEnum.Approved);
            myStatsAsInstructorResult.rejectedCourses = courses.Count(c => c.CourseStatus == CourseStatusEnum.Rejected);
            myStatsAsInstructorResult.pendingCourses = courses.Count(c => c.CourseStatus == CourseStatusEnum.Pending);
            myStatsAsInstructorResult.totalLessons = courses.Sum(c => c.NumberOfLessons);
            myStatsAsInstructorResult.TotalCourses = courses.Count();
            myStatsAsInstructorResult.AverageRate = courses.Average(c => c.AverageRating);
            myStatsAsInstructorResult.totalStudentsEnrolled = ListOfUsersCourse.Count();
            myStatsAsInstructorResult.totalRatings = (int)ListOfUsersCourse.Sum(a => a.Rating);

            ;

            return Success(myStatsAsInstructorResult);



        }

    }
}

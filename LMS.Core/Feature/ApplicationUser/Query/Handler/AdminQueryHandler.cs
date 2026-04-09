using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.ApplicationUser.Query.Models.AdminModel;
using LMS.Core.Feature.ApplicationUser.Query.Result.AdminResult;
using LMS.Core.Resources;
using LMS.Core.Wrappers;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.ApplicationUser.Query.Handler
{
    public class AdminQueryHandler : ResponseHandler, IRequestHandler<GetAllUsersQuery, Response<List<GetUsersQueryResult>>>,
        IRequestHandler<GetUserByIdQuery, Response<GetUserByIdQueryResult>>
        , IRequestHandler<GetAllUserPaginatedQuery, PaginatedResult<GetAllUserPaginatedResponse>>

    {
        private readonly IMapper mapper;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly SignInManager<Users> signInManager;
        private readonly IUserService userService;

        public AdminQueryHandler(IMapper mapper, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, SignInManager<Users> signInManager, IUserService userService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.signInManager = signInManager;
            this.userService = userService;
        }

        public async Task<Response<List<GetUsersQueryResult>>> Handle(
     GetAllUsersQuery request,
     CancellationToken cancellationToken)
        {
            var users = await userManager.Users.ToListAsync(cancellationToken);

            if (!users.Any())
                return NotFound<List<GetUsersQueryResult>>("No users found.");

            var result = mapper.Map<List<GetUsersQueryResult>>(users);

            return Success(result);
        }

        public async Task<Response<GetUserByIdQueryResult>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var UserWithEmailIsFound = await userManager.FindByIdAsync(request.Id);
            if (UserWithEmailIsFound == null)
                return NotFound<GetUserByIdQueryResult>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var result = mapper.Map<GetUserByIdQueryResult>(UserWithEmailIsFound);
            result.RoleName = (await userManager.GetRolesAsync(UserWithEmailIsFound)).FirstOrDefault();
            return Success(result);
        }

        public async Task<PaginatedResult<GetAllUserPaginatedResponse>> Handle(GetAllUserPaginatedQuery request, CancellationToken cancellationToken)
        {
            var query = userService.GetAllUsers();

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var mapped = mapper.Map<List<GetAllUserPaginatedResponse>>(users);

            return PaginatedResult<GetAllUserPaginatedResponse>.Success(mapped, totalCount, request.PageNumber, request.PageSize);
        }
    }
}

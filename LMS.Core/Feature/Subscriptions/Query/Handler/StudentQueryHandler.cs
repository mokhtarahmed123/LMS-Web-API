using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Subscriptions.Query.Model;
using LMS.Core.Feature.Subscriptions.Query.Model.StudentModel;
using LMS.Core.Feature.Subscriptions.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Query.Handler
{
    public class StudentQueryHandler : ResponseHandler,
        IRequestHandler<GetAllMySubscriptionsQuery, Response<List<GetMySubscriptionResult>>>,
        IRequestHandler<GetAllMyRequestsSubscriptionsQuery, Response<List<GetAllMyRequestsSubscriptionsResult>>>
    {
        private readonly IMapper mapper;
        private readonly IPlanService planService;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly ICurrentUserService currentUserService;

        public StudentQueryHandler(IMapper mapper, IPlanService planService, ISubscriptionsService subscriptionsService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer, ICurrentUserService currentUserService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.planService = planService;
            this.subscriptionsService = subscriptionsService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
            this.currentUserService = currentUserService;
        }
        public async Task<Response<List<GetMySubscriptionResult>>> Handle(GetAllMySubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;
            if (UserId == null) return BadRequest<List<GetMySubscriptionResult>>(stringLocalizer[SharedResourcesKeys.PleaseLogIn]);
            var User = await userManager.FindByIdAsync(UserId);
            if (User == null) return NotFound<List<GetMySubscriptionResult>>(stringLocalizer[SharedResourcesKeys.UserNotFound]);

            var AllSubscription = await subscriptionsService.GetMySubscriptions(UserId);
            if (!AllSubscription.Any())
                return NotFound<List<GetMySubscriptionResult>>();
            var AllSubscriptionMapped = mapper.Map<List<GetMySubscriptionResult>>(AllSubscription);
            return Success(AllSubscriptionMapped);
        }

        public async Task<Response<List<GetAllMyRequestsSubscriptionsResult>>> Handle(GetAllMyRequestsSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var UserId = currentUserService.UserId;

            if (UserId == null) return BadRequest<List<GetAllMyRequestsSubscriptionsResult>>(stringLocalizer[SharedResourcesKeys.PleaseLogIn]);
            var User = await userManager.FindByIdAsync(UserId);
            if (User == null) return NotFound<List<GetAllMyRequestsSubscriptionsResult>>(stringLocalizer[SharedResourcesKeys.UserNotFound]);
            var AllRequestsSubscriptions = await subscriptionsService.GetAllSubscriptionsRequestsByUserId(UserId);
            if (!AllRequestsSubscriptions.Any())
                return NotFound<List<GetAllMyRequestsSubscriptionsResult>>();
            var AllRequestsSubscriptionsMapped = mapper.Map<List<GetAllMyRequestsSubscriptionsResult>>(AllRequestsSubscriptions);
            return Success(AllRequestsSubscriptionsMapped);
        }
    }
}

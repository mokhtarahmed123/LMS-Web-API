using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Subscriptions.Command.Model;
using LMS.Core.Resources;
using LMS.Data_.Entities;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Subscriptions.Command.Handler
{
    public class AdminCommandHandler : ResponseHandler,
        IRequestHandler<DeleteSubscriptionsCommand, Response<string>>

    {
        private readonly IMapper mapper;
        private readonly ISubscriptionsService subscriptionsService;
        private readonly IPlanService planService;
        private readonly UserManager<Users> userManager;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminCommandHandler(IMapper mapper, ISubscriptionsService subscriptionsService, IPlanService planService, UserManager<Users> userManager, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.subscriptionsService = subscriptionsService;
            this.planService = planService;
            this.userManager = userManager;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<Response<string>> Handle(DeleteSubscriptionsCommand request, CancellationToken cancellationToken)
        {
            var Subscription = await subscriptionsService.Get(request.Id);
            if (Subscription == null) return NotFound<string>();
            var Result = await subscriptionsService.Delete(request.Id);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>();
        }
    }
}

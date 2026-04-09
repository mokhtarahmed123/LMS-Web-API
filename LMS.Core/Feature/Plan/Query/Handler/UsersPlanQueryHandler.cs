using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Plan.Query.Models;
using LMS.Core.Feature.Plan.Query.Result;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Plan.Query.Handler
{
    public class UsersPlanQueryHandler : ResponseHandler, IRequestHandler<GetAllPlanQueryForUsers, Response<List<GetAllPlanResultForUsers>>>
    {
        private readonly IMapper mapper;
        private readonly IPlanService planService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public UsersPlanQueryHandler(IMapper mapper, IPlanService planService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.planService = planService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<Response<List<GetAllPlanResultForUsers>>> Handle(GetAllPlanQueryForUsers request, CancellationToken cancellationToken)
        {
            var ListOfPlan = await planService.GetAll();
            if (!ListOfPlan.Any())
                return NotFound<List<GetAllPlanResultForUsers>>(stringLocalizer[SharedResourcesKeys.PlanNotFound]);
            var Plans = mapper.Map<List<GetAllPlanResultForUsers>>(ListOfPlan);
            return Success(Plans);
        }
    }
}

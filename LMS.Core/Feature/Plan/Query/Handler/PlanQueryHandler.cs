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
    public class PlanQueryHandler : ResponseHandler, IRequestHandler<GetPlanByIdQuery, Response<GetPlanByIdResult>>
    {
        private readonly IMapper mapper;
        private readonly IPlanService planService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public PlanQueryHandler(IMapper mapper, IPlanService planService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.planService = planService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<Response<GetPlanByIdResult>> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            var Plan = await planService.Get(request.Id);
            if (Plan == null) return NotFound<GetPlanByIdResult>(stringLocalizer[SharedResourcesKeys.PlanNotFound]);

            var PlanMapped = mapper.Map<GetPlanByIdResult>(Plan);
            return Success(PlanMapped);

        }
    }
}

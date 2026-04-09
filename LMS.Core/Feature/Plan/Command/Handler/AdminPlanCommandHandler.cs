using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Plan.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
namespace LMS.Core.Feature.Plan.Command.Handler
{
    public class AdminPlanCommandHandler : ResponseHandler,
        IRequestHandler<AddPlanCommand, Response<string>>,
        IRequestHandler<UpdatePlanCommand, Response<string>>,
        IRequestHandler<DeletePlanCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly IPlanService planService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminPlanCommandHandler(IMapper mapper, IPlanService planService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.planService = planService;
            this.stringLocalizer = stringLocalizer;
        }

        public async Task<Response<string>> Handle(AddPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = mapper.Map<LMS.Data_.Entities.Plan>(request);
            var Result = await planService.Add(plan);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedAdded]);
            return Created<string>(stringLocalizer[SharedResourcesKeys.PlanAdded]);
        }
        public async Task<Response<string>> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var PlanIsFound = await planService.Get(request.Id);
            if (PlanIsFound == null) return NotFound<string>();
            var plan = mapper.Map(request, PlanIsFound);
            var Result = await planService.Update(plan);
            if (Result == null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>(stringLocalizer[SharedResourcesKeys.PlanUpdated]);
        }
        public async Task<Response<string>> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {
            var PlanIsFound = await planService.Get(request.Id);
            if (PlanIsFound == null) return NotFound<string>();
            var Result = await planService.Delete(request.Id);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>(stringLocalizer[SharedResourcesKeys.PlanDeleted]);
        }
    }
}

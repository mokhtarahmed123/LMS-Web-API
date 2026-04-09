using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Coupons.Command.Models;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Coupons.Command.Handler
{
    public class AdminCommandHandler : ResponseHandler,
        IRequestHandler<CreateCouponCommand, Response<string>>,
        IRequestHandler<UpdateCouponCommand, Response<string>>,
        IRequestHandler<DeleteCouponCommand, Response<string>>,
        IRequestHandler<ChangeStatusOfCouponCommand, Response<string>>
    {
        private readonly IMapper mapper;
        private readonly ICouponsService couponsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminCommandHandler(IMapper mapper, ICouponsService couponsService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.couponsService = couponsService;
            this.stringLocalizer = stringLocalizer;
        }
        public async Task<Response<string>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            var listOfCoupons = new List<LMS.Data_.Entities.Coupons>();

            for (int i = 0; i < request.CountOfCoupons; i++)
            {
                var mapped = mapper.Map<LMS.Data_.Entities.Coupons>(request);
                mapped.Code = Guid.NewGuid().ToString("N")[..8].ToUpper();
                listOfCoupons.Add(mapped);
            }

            await couponsService.AddCoupons(listOfCoupons);

            return Created<string>($"{listOfCoupons.Count} coupons created successfully");
        }

        public async Task<Response<string>> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Coupon = await couponsService.GetByID(request.Id);
            if (Coupon == null) return NotFound<string>();
            var Result = await couponsService.Delete(request.Id);
            if (!Result) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedDeleted]);
            return Deleted<string>();
        }

        public async Task<Response<string>> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Coupon = await couponsService.GetByID(request.Id);
            if (Coupon == null) return NotFound<string>();

            var Mapped = mapper.Map(request, Coupon);
            var Result = await couponsService.Update(Mapped);
            if (Result != "Updated") return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>();
        }

        public async Task<Response<string>> Handle(ChangeStatusOfCouponCommand request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
                return BadRequest<string>(stringLocalizer[SharedResourcesKeys.InvalidId]);
            var Coupon = await couponsService.GetByID(request.Id);
            if (Coupon == null) return NotFound<string>();

            Coupon.IsActive = request.IsActive;
            var Result = await couponsService.Update(Coupon);
            if (Result != "Updated") return BadRequest<string>(stringLocalizer[SharedResourcesKeys.FailedUpdated]);
            return Updated<string>();

        }
    }
}

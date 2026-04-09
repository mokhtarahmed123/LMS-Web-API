using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Feature.Coupons.Query.Models;
using LMS.Core.Feature.Coupons.Query.Result;
using LMS.Core.Resources;
using LMS.Core.Wrappers;
using LMS.Service.Abstract;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace LMS.Core.Feature.Coupons.Query.Handler
{
    public class AdminQueryHandler : ResponseHandler,
        IRequestHandler<GetAllQuery, Response<List<GetAllCouponResult>>>,
        IRequestHandler<GetAllExpiredQuery, Response<List<GetAllExpiredResult>>>
        , IRequestHandler<GetByCodeQuery, Response<GetByCodeResult>>
        , IRequestHandler<GetAllCouponQueryPaginated, PaginatedResult<GetAllCouponQueryPaginatedResult>>



    {
        private readonly IMapper mapper;
        private readonly ICouponsService couponsService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;

        public AdminQueryHandler(IMapper mapper, ICouponsService couponsService, IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.couponsService = couponsService;
            this.stringLocalizer = stringLocalizer;
        }


        public async Task<Response<List<GetAllCouponResult>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
        {
            var List = await couponsService.GetAllCoupons(request.IsActive);
            if (!List.Any()) return NotFound<List<GetAllCouponResult>>();
            var Mapped = mapper.Map<List<GetAllCouponResult>>(List);
            int Count = List.Count;
            return Success(Mapped, Count);
        }

        public async Task<Response<List<GetAllExpiredResult>>> Handle(GetAllExpiredQuery request, CancellationToken cancellationToken)
        {
            var List = await couponsService.GetAllExpiredAsync();
            if (!List.Any()) return NotFound<List<GetAllExpiredResult>>();
            var Mapped = mapper.Map<List<GetAllExpiredResult>>(List);
            int Count = List.Count;
            return Success(Mapped, Count);


        }

        public async Task<Response<GetByCodeResult>> Handle(GetByCodeQuery request, CancellationToken cancellationToken)
        {
            var Coupon = await couponsService.GetByCode(request.Code);
            if (Coupon == null) return NotFound<GetByCodeResult>();
            var Mapped = mapper.Map<GetByCodeResult>(Coupon);
            return Success<GetByCodeResult>(Mapped);
        }

        public async Task<PaginatedResult<GetAllCouponQueryPaginatedResult>> Handle(GetAllCouponQueryPaginated request, CancellationToken cancellationToken)
        {
            Expression<Func<LMS.Data_.Entities.Coupons, GetAllCouponQueryPaginatedResult>> expression = e => new
            GetAllCouponQueryPaginatedResult(e.Id, e.Code, e.DiscountType, e.DiscountValue, e.StartDate,
            e.EndDate, e.UsageLimit, e.UsedCount, e.IsActive, e.CreatedAt);
            var quarable = couponsService.GetAllCouponsQueryable();
            var PaginatedList = await quarable.Select(expression).ToPaginatedListAsync((int)request.PageNumber, (int)request.PageSize);
            return PaginatedList;

        }
    }
}

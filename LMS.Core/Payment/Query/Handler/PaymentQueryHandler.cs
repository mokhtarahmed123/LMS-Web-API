using AutoMapper;
using LMS.Core.Bases;
using LMS.Core.Payment.Query.Model;
using LMS.Core.Payment.Query.Result;
using LMS.Core.Resources;
using LMS.Data_;
using LMS.Service;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Payment.Query.Handler
{
    public class PaymentQueryHandler : ResponseHandler,
        IRequestHandler<GetMyPaymentsHistory, Response<List<PaymentHistoryItemResult>>>
    {
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IPaymobService paymobService;

        public PaymentQueryHandler(IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<SharedResources> stringLocalizer, IPaymobService paymobService) : base(stringLocalizer)
        {
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.stringLocalizer = stringLocalizer;
            this.paymobService = paymobService;
        }



        public async Task<Response<List<PaymentHistoryItemResult>>> Handle(GetMyPaymentsHistory request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var paymentsHistory = await paymobService.MyHistory(userId);

            if (!paymentsHistory.Any())
                return NotFound<List<PaymentHistoryItemResult>>();

            var result = mapper.Map<List<PaymentHistoryItemResult>>(paymentsHistory);
            return Success(result);
        }
    }
}

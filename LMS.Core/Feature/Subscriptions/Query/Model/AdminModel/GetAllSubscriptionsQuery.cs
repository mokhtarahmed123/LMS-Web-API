using LMS.Core.Feature.Subscriptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Subscriptions.Query.Model.AdminModel
{
    public record GetAllSubscriptionsQuery() : IRequest<Response<GetAllSubscriptionsResponse>>;

}

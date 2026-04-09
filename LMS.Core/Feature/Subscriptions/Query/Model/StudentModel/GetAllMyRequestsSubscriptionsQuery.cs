using LMS.Core.Feature.Subscriptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Subscriptions.Query.Model
{
    public record GetAllMyRequestsSubscriptionsQuery : IRequest<Response<List<GetAllMyRequestsSubscriptionsResult>>>;


}

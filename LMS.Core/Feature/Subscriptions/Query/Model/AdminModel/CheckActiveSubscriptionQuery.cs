using LMS.Core.Feature.Subscriptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Subscriptions.Query.Model.AdminModel
{
    public record CheckActiveSubscriptionQuery(string UserId) : IRequest<Response<CheckActiveSubscriptionResult>>;
}

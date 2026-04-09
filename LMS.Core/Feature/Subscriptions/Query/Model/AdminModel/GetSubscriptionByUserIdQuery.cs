using LMS.Core.Feature.Subscriptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Subscriptions.Query.Model.AdminModel
{
    public record GetSubscriptionByUserIdQuery(string UserId) : IRequest<Response<GetSubscriptionByUserIdResponse>>;
    // Admin can view subscription details of a specific user by their UserId
}

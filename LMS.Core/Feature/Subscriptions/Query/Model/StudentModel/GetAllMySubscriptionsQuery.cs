using LMS.Core.Feature.Subscriptions.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Subscriptions.Query.Model.StudentModel
{
    public record GetAllMySubscriptionsQuery : IRequest<Response<List<GetMySubscriptionResult>>>;


}

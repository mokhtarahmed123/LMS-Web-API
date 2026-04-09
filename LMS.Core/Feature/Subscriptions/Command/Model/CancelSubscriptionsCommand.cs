using MediatR;

namespace LMS.Core.Feature.Subscriptions.Command.Model
{
    public record CancelSubscriptionsCommand(int Id) : IRequest<Response<string>>;

}

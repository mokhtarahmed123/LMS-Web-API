using MediatR;

namespace LMS.Core.Feature.Subscriptions.Command.Model
{
    public record DeleteSubscriptionsCommand(int Id) : IRequest<Response<string>>;

}

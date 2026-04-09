using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record RejectInstructorRoleRequestCommand(int RequestId, string? Reason) : IRequest<Response<string>>;

}

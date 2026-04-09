using MediatR;

namespace LMS.Core.Feature.Authorization.Command.Models
{
    public record AcceptInstructorRoleRequestCommand(int RequestId) : IRequest<Response<string>>;





}


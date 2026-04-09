using MediatR;

namespace LMS.Core.Feature.InstructorProfiles.Command.Models
{
    public record CancelRequestInstructorProfileCommand : IRequest<Response<string>>
 ;
}

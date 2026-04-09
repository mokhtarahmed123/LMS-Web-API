using MediatR;

namespace LMS.Core.Feature.InstructorProfiles.Command.Models
{
    public record DeleteInstructorProfileCommand(int Id) : IRequest<Response<string>>;

}

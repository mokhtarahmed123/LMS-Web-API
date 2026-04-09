using MediatR;
using Microsoft.AspNetCore.Http;

namespace LMS.Core.Feature.InstructorProfiles.Command.Models
{
    public class UpdateInstructorProfileCommand : IRequest<Response<string>>
    {
        public string? Bio { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string LinkedInUrl { get; set; }
        public string Email { get; set; }

    }
}

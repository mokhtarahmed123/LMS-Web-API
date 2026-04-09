using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LMS.Core.Feature.InstructorProfiles.Command.Models
{
    public class AddInstructorProfileCommand : IRequest<Response<string>>
    {
        [StringLength(1500)]
        public string? Bio { get; set; }
        public IFormFile? ProfilePicture { get; set; }

        public string LinkedInUrl { get; set; }

        public string Email { get; set; }

    }
}

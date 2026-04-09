using MediatR;
using System.Text.Json.Serialization;
namespace LMS.Core.Feature.UserCourses.Command.Models.Student
{
    public class EnrollUserCourseCommand : IRequest<Response<string>>
    {
        public int CourseId { get; set; }
        // Assuming you have a way to identify the user, such as through authentication context


        [JsonIgnore]
        public DateTime? LastAccessedAt { get; set; } = DateTime.UtcNow;


    }
}


using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.UserCourses.Command.Models.Student
{
    public class RateUserCourseCommand : IRequest<Response<string>>
    {

        public int CourseId { get; set; }
        public int Rating { get; set; }

        [JsonIgnore]
        public DateTime? LastAccessedAt { get; set; } = DateTime.UtcNow;


    }
}

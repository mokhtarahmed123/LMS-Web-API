using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.Quizzes.Command.Model
{
    public class UpdateQuizCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public int TotalMarks { get; set; }
        public int PassingScore { get; set; }

        public DateTime? StartDate { get; set; }
        public bool IsTimeBound { get; set; }

        public DateTime? EndDate { get; set; }

        public int CourseId { get; set; }



    }
}

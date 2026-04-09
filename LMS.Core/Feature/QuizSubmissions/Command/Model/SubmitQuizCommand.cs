using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.QuizSubmissions.Command.Model
{
    public class SubmitQuizCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int QuizId { get; set; }
        public List<AnswersQuiz> answersQuizzes { get; set; }
    }

    public class AnswersQuiz
    {
        public int SelectOption { get; set; }
        public int QuestionId { get; set; }
    }

}

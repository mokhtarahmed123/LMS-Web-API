using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.QuizQuestions.Command.Model
{
    public class UpdateQuizQuestionsCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public int QuizId { get; set; }

        public string QuestionText { get; set; }
        public TypeOfQuestions TypeOfQuestions { get; set; }

    }
}

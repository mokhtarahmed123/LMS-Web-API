using LMS.Data_.Enum;
using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.QuizQuestions.Command.Model
{
    public class AddQuizQuestionsCommand : IRequest<Response<string>>
    {
        public string QuestionText { get; set; }
        public TypeOfQuestions TypeOfQuestions { get; set; }

        [JsonIgnore]
        public int QuizId { get; set; }


    }
}

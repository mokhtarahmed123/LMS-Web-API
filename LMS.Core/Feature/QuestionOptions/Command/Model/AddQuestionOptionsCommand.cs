using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.QuestionOptions.Command.Model
{
    public class AddQuestionOptionsCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int QuizQuestionId { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }



    }
}

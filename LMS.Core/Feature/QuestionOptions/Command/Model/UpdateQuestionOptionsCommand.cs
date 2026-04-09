using MediatR;
using System.Text.Json.Serialization;

namespace LMS.Core.Feature.QuestionOptions.Command.Model
{
    public class UpdateQuestionOptionsCommand : IRequest<Response<string>>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }


    }
}

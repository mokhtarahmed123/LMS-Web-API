using LMS.Core.Feature.QuestionOptions.Command.Model;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.QuestionOptionsMapping
{
    public partial class QuestionOptionsProfile
    {
        private void Update()
        {
            CreateMap<UpdateQuestionOptionsCommand, QuestionOptions>()
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(dest => dest.OptionText, opt => opt.MapFrom(src => src.OptionText));

        }
    }
}

using LMS.Core.Feature.QuestionOptions.Query.Result;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.QuestionOptionsMapping
{
    public partial class QuestionOptionsProfile
    {
        private void GetQuestionOptionsByQuestionId()
        {
            CreateMap<QuestionOptions, GetQuestionOptionsByQuestionIdResult>()
            .ForMember(dest => dest.NumberOfOption, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
            .ForMember(dest => dest.OptionText, opt => opt.MapFrom(src => src.OptionText));

        }
    }
}

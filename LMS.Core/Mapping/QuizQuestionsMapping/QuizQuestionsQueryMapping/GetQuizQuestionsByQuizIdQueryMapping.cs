using LMS.Core.Feature.QuizQuestions.Query.Result;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.QuizQuestionsMapping
{
    public partial class QuizQuestionsProfile
    {
        private void GetQuizQuestionsByQuizId()
        {

            CreateMap<QuizQuestions, GetQuizQuestionsByQuizIdResult>()
               .ForMember(dest => dest.NumberOfQuestion, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
               .ForMember(dest => dest.TypeOfQuestion, opt => opt.MapFrom(src => src.QuestionsType.ToString()))
               ;
        }
    }
}

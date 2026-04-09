using LMS.Core.Feature.QuizQuestions.Command.Model;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.QuizQuestionsMapping
{
    public partial class QuizQuestionsProfile
    {
        private void update()
        {
            CreateMap<UpdateQuizQuestionsCommand, QuizQuestions>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.QuestionText))
                .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.QuestionsType, opt => opt.MapFrom(src => src.TypeOfQuestions));
        }
    }
}

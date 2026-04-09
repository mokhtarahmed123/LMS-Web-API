using AutoMapper;

namespace LMS.Core.Mapping.QuizQuestionsMapping
{
    public partial class QuizQuestionsProfile : Profile
    {
        public QuizQuestionsProfile()
        {
            Add();
            update();
            GetQuizQuestionsByQuizId();
            GetQuizQuestionsById();
        }
    }
}

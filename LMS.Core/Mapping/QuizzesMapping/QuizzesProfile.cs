using AutoMapper;

namespace LMS.Core.Mapping.QuizzesMapping
{
    public partial class QuizzesProfile : Profile
    {
        public QuizzesProfile()
        {
            AddQuiz();
            Update();
            GetAllQuizzes();
            GetAllQuizzesByCourseid();
            GetById();
        }
    }
}

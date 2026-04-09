using LMS.Data_.Entities.Quiz;

namespace LMS.Service.Abstract.QuizzesAbstract
{
    public interface IQuizService
    {


        Task<List<Quizzes>> GetAllQuizzesByCourseIdAsync(int courseId);
        Task<Quizzes> GetQuizByIdAsync(int quizId);
        public IQueryable<Quizzes> GetAllQuizzesAsync();

        Task<Quizzes> AddQuizAsync(Quizzes quiz);
        Task UpdateQuizAsync(Quizzes quiz);
        Task<bool> DeleteQuizAsync(int quizId);







    }
}

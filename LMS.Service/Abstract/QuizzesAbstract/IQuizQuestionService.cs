using LMS.Data_.Entities.Quiz;

namespace LMS.Service.Abstract.QuizzesAbstract
{
    public interface IQuizQuestionService
    {
        Task<QuizQuestions> AddQuestionAsync(QuizQuestions question);
        Task<QuizQuestions> GetQuestionByIdAsync(int questionId);
        Task<List<QuizQuestions>> GetQuestionsByQuizIdAsync(int quizId);
        Task<QuizQuestions> UpdateQuestionAsync(QuizQuestions question);
        Task<bool> DeleteQuestionAsync(int questionId);

    }
}

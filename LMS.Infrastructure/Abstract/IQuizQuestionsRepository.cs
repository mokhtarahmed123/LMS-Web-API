using LMS.Data_.Entities.Quiz;

namespace LMS.Infrastructure.Abstract
{
    public interface IQuizQuestionsRepository : IGenericRepositoryAsync<QuizQuestions>
    {
        Task<List<QuizQuestions>> GetQuestionsByQuizIdAsync(int quizId);
    }
}

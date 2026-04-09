using LMS.Data_.Entities.Quiz;

namespace LMS.Infrastructure.Abstract
{
    public interface IQuestionOptionsRepository : IGenericRepositoryAsync<QuestionOptions>
    {
        Task<List<QuestionOptions>> GetOptionsByQuestionIdAsync(int questionId);
        Task<List<QuestionOptions>> GetCorrectAnswersByQuestionIdsAsync(List<int> questionIds);
    }
}

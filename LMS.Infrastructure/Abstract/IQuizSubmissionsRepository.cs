using LMS.Data_.Entities.Quiz;

namespace LMS.Infrastructure.Abstract
{
    public interface IQuizSubmissionsRepository : IGenericRepositoryAsync<QuizSubmissions>
    {
        Task<List<QuizSubmissions>> GetSubmissionsByUserIdAsync(string userId);

    }
}

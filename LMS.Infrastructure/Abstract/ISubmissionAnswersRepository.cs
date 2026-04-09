using LMS.Data_.Entities.Quiz;

namespace LMS.Infrastructure.Abstract
{
    public interface ISubmissionAnswersRepository : IGenericRepositoryAsync<SubmissionAnswers>
    {
        public Task<List<SubmissionAnswers>> GetAllSubmissionAnswers(string UserId);
    }
}

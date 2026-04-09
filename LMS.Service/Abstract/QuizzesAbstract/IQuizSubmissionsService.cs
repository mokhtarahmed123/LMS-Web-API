using LMS.Data_.Entities.Quiz;

namespace LMS.Service.Abstract.QuizzesAbstract
{
    public interface IQuizSubmissionsService
    {
        Task<QuizSubmissions> SubmitQuizAsync(QuizSubmissions submission, List<SubmissionAnswers> answers);
        Task<bool> DeleteSubmissionAsync(int submissionId);
        Task<List<QuizSubmissions>> GetAllSubmissionAnswers(string UserId);
    }
}

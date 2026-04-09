using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;

namespace LMS.Service.implementation.Quizzesimplementation
{
    public class QuizSubmissionsService : IQuizSubmissionsService
    {
        private readonly IQuizSubmissionsRepository quizSubmissionsRepository;
        private readonly ISubmissionAnswersRepository submissionAnswersRepository;

        public QuizSubmissionsService(IQuizSubmissionsRepository quizSubmissionsRepository, ISubmissionAnswersRepository submissionAnswersRepository)
        {
            this.quizSubmissionsRepository = quizSubmissionsRepository;
            this.submissionAnswersRepository = submissionAnswersRepository;
        }
        public Task<bool> DeleteSubmissionAsync(int submissionId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<QuizSubmissions>> GetAllSubmissionAnswers(string UserId)
        {
            return await quizSubmissionsRepository.GetSubmissionsByUserIdAsync(UserId);

        }

        public async Task<QuizSubmissions> SubmitQuizAsync(
            QuizSubmissions submission,
            List<SubmissionAnswers> answers)
        {
            using var transaction = submissionAnswersRepository.BeginTransaction();

            try
            {

                var submissionReturned = await quizSubmissionsRepository.AddAsync(submission);


                foreach (var answer in answers)
                {
                    answer.SubmissionId = submissionReturned.Id;
                }


                await submissionAnswersRepository.AddRangeAsync(answers);

                await transaction.CommitAsync();

                return submissionReturned;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

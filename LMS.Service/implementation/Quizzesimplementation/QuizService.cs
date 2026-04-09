using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;

namespace LMS.Service.implementation.Quizzesimplementation
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository quizRepository;

        public QuizService(IQuizRepository quizRepository)
        {
            this.quizRepository = quizRepository;
        }

        public async Task<Quizzes> AddQuizAsync(Quizzes quiz)
        {
            var AddedQuiz = await quizRepository.AddAsync(quiz);
            return AddedQuiz;

        }

        public async Task<bool> DeleteQuizAsync(int quizId)
        {
            using var transaction = quizRepository.BeginTransaction();
            try
            {
                var quiz = quizRepository.GetByIdAsync(quizId).Result;
                if (quiz == null)
                {
                    return false;
                }
                await quizRepository.DeleteAsync(quiz);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }

        }

        public IQueryable<Quizzes> GetAllQuizzesAsync()
        {
            return quizRepository.GetTableNoTracking();


        }

        public async Task<List<Quizzes>> GetAllQuizzesByCourseIdAsync(int courseId)
        {
            return await quizRepository.GetAllQuizzesByCourseIdAsync(courseId);
        }

        public async Task<Quizzes> GetQuizByIdAsync(int quizId)
        {
            return await quizRepository.GetByIdAsync(quizId);

        }

        public async Task UpdateQuizAsync(Quizzes quiz)
        {

            await quizRepository.UpdateAsync(quiz);
        }
    }
}

using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;

namespace LMS.Service.implementation.Quizzesimplementation
{
    public class QuizQuestionService : IQuizQuestionService
    {
        private readonly IQuizQuestionsRepository quizQuestionsRepository;

        public QuizQuestionService(IQuizQuestionsRepository quizQuestionsRepository)
        {
            this.quizQuestionsRepository = quizQuestionsRepository;
        }


        public async Task<QuizQuestions> AddQuestionAsync(QuizQuestions question)
        {
            var addedQuestion = await quizQuestionsRepository.AddAsync(question);
            return addedQuestion;

        }

        public async Task<bool> DeleteQuestionAsync(int questionId)
        {
            using var transaction = quizQuestionsRepository.BeginTransaction();
            try
            {
                var question = await quizQuestionsRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return (false);
                }
                await quizQuestionsRepository.DeleteAsync(question);
                transaction.Commit();
                return (true);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<QuizQuestions> GetQuestionByIdAsync(int questionId)
        {
            return await quizQuestionsRepository.GetByIdAsync(questionId);

        }

        public async Task<List<QuizQuestions>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await quizQuestionsRepository.GetQuestionsByQuizIdAsync(quizId);

        }

        public async Task<QuizQuestions> UpdateQuestionAsync(QuizQuestions question)
        {
            await quizQuestionsRepository.UpdateAsync(question);
            return question;

        }
    }
}

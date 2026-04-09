using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract.QuizzesAbstract;

namespace LMS.Service.implementation.Quizzesimplementation
{
    public class QuestionOptionsService : IQuestionOptionsService
    {
        private readonly IQuestionOptionsRepository questionOptionsRepository;

        public QuestionOptionsService(IQuestionOptionsRepository questionOptionsRepository)
        {
            this.questionOptionsRepository = questionOptionsRepository;
        }
        public async Task<QuestionOptions> AddOptionAsync(QuestionOptions option)
        {
            return await questionOptionsRepository.AddAsync(option);
        }
        public async Task<bool> DeleteOptionAsync(int optionId)
        {
            using var transaction = questionOptionsRepository.BeginTransaction();
            try
            {
                var option = await questionOptionsRepository.GetByIdAsync(optionId);
                if (option == null)
                {
                    return (false);
                }
                await questionOptionsRepository.DeleteAsync(option);
                transaction.Commit();
                return (true);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<List<QuestionOptions>> GetCorrectAnswersByQuestionIdsAsync(List<int> questionIds)
        {
            return await questionOptionsRepository.GetCorrectAnswersByQuestionIdsAsync(questionIds);
        }

        public async Task<QuestionOptions> GetOptionByIdAsync(int optionId)
        {
            return await questionOptionsRepository.GetByIdAsync(optionId);
        }

        public async Task<List<QuestionOptions>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await questionOptionsRepository.GetOptionsByQuestionIdAsync(questionId);

        }

        public async Task<QuestionOptions> UpdateOptionAsync(QuestionOptions option)
        {
            await questionOptionsRepository.UpdateAsync(option);
            return option;
        }
    }
}

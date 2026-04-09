using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class QuestionOptionsRepository : GenericRepositoryAsync<QuestionOptions>, IQuestionOptionsRepository
    {
        private readonly DbSet<QuestionOptions> QuestionOptionsRepo;
        public QuestionOptionsRepository(AppDbContext dbContext) : base(dbContext)
        {
            QuestionOptionsRepo = dbContext.Set<QuestionOptions>();
        }

        public async Task<List<QuestionOptions>> GetCorrectAnswersByQuestionIdsAsync(List<int> questionIds)
        {
            return await QuestionOptionsRepo
                .Where(o => questionIds.Contains(o.QuizQuestionId) && o.IsCorrect)
                .ToListAsync();
        }

        public async Task<List<QuestionOptions>> GetOptionsByQuestionIdAsync(int questionId)
        {
            return await QuestionOptionsRepo.Where(qo => qo.QuizQuestionId == questionId).ToListAsync();
        }
    }
}

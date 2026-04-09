using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class QuizQuestionsRepository : GenericRepositoryAsync<QuizQuestions>, IQuizQuestionsRepository
    {
        private readonly DbSet<QuizQuestions> quizQuestions;
        public QuizQuestionsRepository(AppDbContext dbContext) : base(dbContext)
        {
            quizQuestions = dbContext.Set<QuizQuestions>();
        }

        public async Task<List<QuizQuestions>> GetQuestionsByQuizIdAsync(int quizId)
        {
            return await quizQuestions.Include(a => a.Options).Where(q => q.QuizId == quizId).ToListAsync();

        }
    }
}

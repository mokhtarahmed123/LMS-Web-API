using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class QuizSubmissionsRepository : GenericRepositoryAsync<QuizSubmissions>, IQuizSubmissionsRepository
    {

        private readonly DbSet<QuizSubmissions> QuizSubmissionRepo;
        public QuizSubmissionsRepository(AppDbContext dbContext) : base(dbContext)
        {
            QuizSubmissionRepo = dbContext.Set<QuizSubmissions>();
        }
        public async Task<List<QuizSubmissions>> GetSubmissionsByUserIdAsync(string userId)
        {
            return await QuizSubmissionRepo
            .Include(s => s.Answers)
            .ThenInclude(a => a.SelectedOption)         
        .Include(s => s.Student)                
        .Include(s => s.Quiz)                   
        .Where(s => s.UserId == userId)
        .ToListAsync();
        }
    }
}

using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class SubmissionAnswersRepository : GenericRepositoryAsync<SubmissionAnswers>, ISubmissionAnswersRepository
    {
        private readonly DbSet<SubmissionAnswers> SubmissionAnswersRepo;
        public SubmissionAnswersRepository(AppDbContext dbContext) : base(dbContext)
        {
            SubmissionAnswersRepo = dbContext.Set<SubmissionAnswers>();
        }

        public async Task<List<SubmissionAnswers>> GetAllSubmissionAnswers(string UserId)
        {
            return await SubmissionAnswersRepo.Include(a => a.Submission).ThenInclude(a => a.Student).Include(a => a.Question).ThenInclude(a => a.Options).Where(A => A.Submission.UserId == UserId).ToListAsync();

        }
    }
}

using LMS.Data_.Entities.Quiz;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class QuizRepository : GenericRepositoryAsync<Quizzes>, IQuizRepository
    {
        private readonly DbSet<Quizzes> Quizzes;

        public QuizRepository(AppDbContext context) : base(context)
        {
            Quizzes = context.Set<Quizzes>();

        }

        public async Task<List<Quizzes>> GetAllQuizzesByCourseIdAsync(int courseId)
        {
            return await Quizzes.Where(q => q.CourseId == courseId).ToListAsync();

        }
    }
}

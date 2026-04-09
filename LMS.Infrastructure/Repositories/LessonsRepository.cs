using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class LessonsRepository : GenericRepositoryAsync<Lessons>, ILessonsRepository
    {
        private readonly DbSet<Lessons> Lessons;

        public LessonsRepository(AppDbContext context) : base(context)
        {
            Lessons = context.Set<Lessons>();
        }

        public async Task<List<Lessons>> GetAllLessonsByCourseId(int courseId)
        {
            return await Lessons.Include(a => a.Course).Where(i => i.CourseId == courseId && i.IsPreview == true).OrderBy(a => a.OrderNumber).ToListAsync();
        }

        public async Task<int> GetCountOfLessonsByCourseId(int courseId)
        {
            return await Lessons.Where(a => a.CourseId == courseId).CountAsync();
        }

        public async Task<Lessons> GetLessonById(int Id)
        {
            return await Lessons.Include(A => A.Course).AsNoTracking().FirstOrDefaultAsync(A => A.Id == Id);

        }

        public async Task<int> GetTotalLessonsByInstructorId(int InstructorId)
        {
            return await Lessons.Include(a => a.Course).Where(a => a.Course.InstructorProfileId == InstructorId).CountAsync();
        }

        public async Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber)
        {
            return await Lessons.AnyAsync(l =>
                l.CourseId == courseId &&
                l.OrderNumber == orderNumber
            );
        }

        public async Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber, int? lessonId = null)
        {
            return await Lessons.AnyAsync(l =>
                l.CourseId == courseId &&
                l.OrderNumber == orderNumber &&
                (!lessonId.HasValue || l.Id != lessonId.Value));
        }

    }
}

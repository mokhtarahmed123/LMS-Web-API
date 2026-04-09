using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class LessonFilesRepository : GenericRepositoryAsync<LessonFiles>, ILessonFilesRepository
    {
        private readonly DbSet<LessonFiles> LessonFiles;
        public LessonFilesRepository(AppDbContext dbContext) : base(dbContext)
        {
            LessonFiles = dbContext.Set<LessonFiles>();
        }

        public IQueryable<LessonFiles> GetAllByLessonId(int lessonId)
        {
            return LessonFiles
                .Where(a => a.LessonId == lessonId)
                .AsNoTracking();
        }

        public IQueryable<LessonFiles> GetAllMyLessonsFilesAsInstructor(int InstructorId)
        {
            return LessonFiles.Include(a => a.Lesson).Where(a => a.Lesson.Course.InstructorProfileId == InstructorId).AsQueryable();
        }
        public async Task<LessonFiles> GetByIdAsync(int Id)
        {
            return await LessonFiles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);

        }
    }
}
using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class CoursesRepository : GenericRepositoryAsync<Courses>, ICoursesRepository
    {
        private readonly DbSet<Courses> Courses;
        public CoursesRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            Courses = appDbContext.Set<Courses>();
        }

        public async Task<List<Courses>> GetAllCoursesAsync()
        {
            return await Courses.Include(c => c.Category).Include(a => a.InstructorProfile).ThenInclude(a => a.User).ToListAsync();
        }

        public async Task<Courses> GetCourseByIdAsync(int id)
        {
            return await Courses.Include(c => c.Category).Include(a => a.InstructorProfile).ThenInclude(a => a.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Courses>> GetCoursesByCategoryIdAsync(int CategoryId)
        {
            return await Courses.Include(c => c.Category).Include(a => a.InstructorProfile).ThenInclude(a => a.User).Where(c => c.CategoryId == CategoryId).ToListAsync();
        }

        public async Task<List<Courses>> GetCoursesByInstructorIdAsync(int InstructorId)
        {
            return await Courses.Include(c => c.Category).Include(a => a.InstructorProfile).ThenInclude(a => a.User).Where(c => c.InstructorProfileId == InstructorId).ToListAsync();
        }

        public async Task<List<Courses>> GetAllCoursesPending()
        {
            return await Courses.Include(c => c.Category).Include(a => a.InstructorProfile).ThenInclude(a => a.User).Where(c => c.CourseStatus == CourseStatusEnum.Pending).ToListAsync();

        }

        public async Task<int> GetCountByCourseState(int InstructorId, CourseStatusEnum CourseStatusEnum)
        {
            return await Courses.Where(a => a.InstructorProfileId == InstructorId && a.CourseStatus == CourseStatusEnum).CountAsync();


        }

        public async Task<int> GetCount(int InstructorId)
        {
            return await Courses.Where(a => a.InstructorProfileId == InstructorId).CountAsync();

        }


    }
}

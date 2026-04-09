using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories
{
    public class UserCoursesRepository : GenericRepositoryAsync<UserCourses>, IUserCoursesRepository
    {
        private readonly DbSet<UserCourses> UserCourse;

        public UserCoursesRepository(AppDbContext context) : base(context)
        {
            UserCourse = context.Set<UserCourses>();
        }

        public async Task<List<UserCourses>> GetAll(int InstructorId)
        {
            return await UserCourse.Include(a => a.Course).ThenInclude(a => a.InstructorProfile).Where(a => a.Course.InstructorProfileId == InstructorId).ToListAsync();
        }

        public async Task<double> GetAverageRatingByCourseId(int courseId)
        {
            return await UserCourse
                   .Where(x => x.CourseId == courseId && x.Rating != null)
                   .AverageAsync(x => x.Rating.Value);
        }

        public async Task<int> GetCountOfRatingByCourseId(int courseId)
        {
            return await UserCourse.Where(uc => uc.CourseId == courseId && uc.Rating.HasValue).CountAsync();
        }

        public async Task<int> GetCountOfStudentByCourseId(int courseId)
        {
            return await UserCourse.Where(uc => uc.CourseId == courseId).CountAsync();

        }

        public async Task<List<UserCourses>> GetFavouriteCoursesByUserIdAsync(string userId)
        {
            return await UserCourse.Include(a => a.User).Include(a => a.Course).ThenInclude(a => a.Category).
                Include(a => a.Course).ThenInclude(a => a.InstructorProfile).ThenInclude(a => a.User)
                .Where(uc => uc.UserId == userId & uc.IsFavorite == true).ToListAsync();
        }

        public async Task<int> GetSumOfRatingByCourseId(int courseId)
        {
            return await UserCourse.Where(uc => uc.CourseId == courseId && uc.Rating.HasValue).SumAsync(uc => uc.Rating.Value);
        }

        public async Task<UserCourses> GetUserCourseAsync(string userId, int courseId)
        {
            return await UserCourse.Include(a => a.User).Include(a => a.Course).FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public Task<List<UserCourses>> GetUserCoursesByCourseIdAsync(int courseId)
        {
            return UserCourse.Include(a => a.User).Include(a => a.Course).Where(uc => uc.CourseId == courseId).ToListAsync();

        }

        public async Task<List<UserCourses>> GetUserCoursesByUserIdAsync(string userId)
        {
            return await UserCourse.Include(a => a.User).Include(a => a.Course).ThenInclude(a => a.Category).
                Include(a => a.Course).ThenInclude(a => a.InstructorProfile).ThenInclude(a => a.User)
                .Where(uc => uc.UserId == userId).ToListAsync();

        }

        public async Task<List<string>> GetUsersIdsByCourseId(int courseId)
        {
            return await UserCourse.Where(a => a.CourseId == courseId).Select(a => a.UserId).ToListAsync();
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId)
        {
            return await UserCourse.Include(a => a.User).Include(a => a.Course).AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }
    }
}

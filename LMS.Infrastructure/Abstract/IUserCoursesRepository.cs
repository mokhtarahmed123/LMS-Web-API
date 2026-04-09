using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface IUserCoursesRepository : IGenericRepositoryAsync<UserCourses>
    {
        public Task<UserCourses> GetUserCourseAsync(string userId, int courseId);
        public Task<List<UserCourses>> GetUserCoursesByUserIdAsync(string userId);
        //public Task<List<UserCourses>> GetUserCoursesByCourseIdAsync(string CourseId);
        public Task<List<UserCourses>> GetFavouriteCoursesByUserIdAsync(string userId);
        public Task<List<UserCourses>> GetUserCoursesByCourseIdAsync(int courseId);
        public Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);
        public Task<int> GetSumOfRatingByCourseId(int courseId);
        public Task<int> GetCountOfRatingByCourseId(int courseId);
        public Task<int> GetCountOfStudentByCourseId(int courseId);
        Task<double> GetAverageRatingByCourseId(int courseId);
        public Task<List<UserCourses>> GetAll(int InstructorId);
        Task<List<string>> GetUsersIdsByCourseId(int courseId);

    }
}

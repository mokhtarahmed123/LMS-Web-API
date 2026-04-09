using LMS.Data_.Entities;

namespace LMS.Service.Abstract
{
    public interface IUserCoursesService
    {
        public Task<UserCourses> Add(UserCourses userCourses);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);
        Task<UserCourses> Update(UserCourses userCourses);
        Task<bool> Delete(string userId, int courseId);
        Task DeleteAllUserCourses(string userId);
        Task<int> GetCountOfRatingByCourseId(int courseId);
        Task<int> GetSumOfRatingByCourseId(int courseId);
        Task<double> GetAverageRatingByCourseId(int courseId);
        Task<List<UserCourses>> GetFavouriteCoursesByUserIdAsync(string userId);
        Task<List<string>> GetUsersIdsByCourseId(int courseId);
        Task<List<UserCourses>> GetUsersCourseIdsByCourseId(int courseId);
        Task<List<UserCourses>> GetUserCoursesByUserIdAsync(string userId);
        Task<UserCourses> GetUserCourseAsync(string userId, int courseId);
        Task<int> GetCountOfStudentByCourseId(int courseId);
        Task<List<UserCourses>> GetAll(int InstructorId);
    }
}

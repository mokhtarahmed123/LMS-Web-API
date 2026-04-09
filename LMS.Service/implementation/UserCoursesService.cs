using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;

namespace LMS.Service.implementation
{
    public class UserCoursesService : IUserCoursesService
    {
        private readonly IUserCoursesRepository userCoursesRepository;

        public UserCoursesService(IUserCoursesRepository userCoursesRepository)
        {
            this.userCoursesRepository = userCoursesRepository;
        }
        public async Task<UserCourses> Add(UserCourses userCourses)
        {
            var User = await userCoursesRepository.AddAsync(userCourses);
            return User;
        }

        public async Task<bool> Delete(string userId, int courseId)
        {
            using var transaction = userCoursesRepository.BeginTransaction();
            try
            {
                var Row = await userCoursesRepository.GetUserCourseAsync(userId, courseId);

                if (Row == null)
                    return false;
                await userCoursesRepository.DeleteAsync(Row);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task DeleteAllUserCourses(string userId)
        {
            using var transaction = userCoursesRepository.BeginTransaction();
            try
            {
                var userCourses = await userCoursesRepository.GetUserCoursesByUserIdAsync(userId);

                if (userCourses != null && userCourses.Any())
                {
                    await userCoursesRepository.DeleteRangeAsync(userCourses);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<List<UserCourses>> GetAll(int InstructorId)
        {
            return userCoursesRepository.GetAll(InstructorId);
        }

        public async Task<double> GetAverageRatingByCourseId(int courseId)
        {
            return await userCoursesRepository.GetAverageRatingByCourseId(courseId);
        }

        public async Task<int> GetCountOfRatingByCourseId(int courseId)
        {
            return await userCoursesRepository.GetCountOfRatingByCourseId(courseId);


        }

        public async Task<int> GetCountOfStudentByCourseId(int courseId)
        {
            return await userCoursesRepository.GetCountOfStudentByCourseId(courseId);
        }

        public async Task<List<UserCourses>> GetFavouriteCoursesByUserIdAsync(string userId)
        {
            return await userCoursesRepository.GetFavouriteCoursesByUserIdAsync(userId);
        }

        public async Task<int> GetSumOfRatingByCourseId(int courseId)
        {
            return await userCoursesRepository.GetSumOfRatingByCourseId(courseId);

        }

        public async Task<UserCourses> GetUserCourseAsync(string userId, int courseId)
        {
            return await userCoursesRepository.GetUserCourseAsync(userId, courseId);
        }

        public async Task<List<UserCourses>> GetUserCoursesByUserIdAsync(string userId)
        {
            return await userCoursesRepository.GetUserCoursesByUserIdAsync(userId);
        }

        public async Task<List<UserCourses>> GetUsersCourseIdsByCourseId(int courseId)
        {
            return await userCoursesRepository.GetUserCoursesByCourseIdAsync(courseId);
        }

        public async Task<List<string>> GetUsersIdsByCourseId(int courseId)
        {
            return await userCoursesRepository.GetUsersIdsByCourseId(courseId);
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId)
        {
            return await userCoursesRepository.IsUserEnrolledInCourseAsync(userId, courseId);
        }

        public async Task<UserCourses> Update(UserCourses userCourses)
        {
            await userCoursesRepository.UpdateAsync(userCourses);
            return userCourses;
        }
    }
}

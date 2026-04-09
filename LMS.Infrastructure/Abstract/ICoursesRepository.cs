using LMS.Data_.Entities;
using LMS.Data_.Enum;
namespace LMS.Infrastructure.Abstract
{
    public interface ICoursesRepository : IGenericRepositoryAsync<Courses>
    {
        Task<List<Courses>> GetAllCoursesAsync();
        Task<Courses> GetCourseByIdAsync(int id);
        Task<List<Courses>> GetCoursesByCategoryIdAsync(int CategoryId);
        Task<List<Courses>> GetCoursesByInstructorIdAsync(int InstructorId);
        Task<List<Courses>> GetAllCoursesPending();

        public Task<int> GetCountByCourseState(int Instructorid, CourseStatusEnum CourseStatusEnum);
        public Task<int> GetCount(int Instructorid);


    }
}

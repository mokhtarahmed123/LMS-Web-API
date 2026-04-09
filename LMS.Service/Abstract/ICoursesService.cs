using LMS.Data_.Entities;
using LMS.Data_.Enum;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.Abstract
{
    public interface ICoursesService
    {
        public Task<List<Courses>> GetAllCoursesAsync();
        public Task<Courses> AddCourse(Courses course, IFormFile File);
        public Task<Courses> GetCourseById(int id);
        Task<List<Courses>> GetCoursesByCategoryIdAsync(int CategoryId);
        Task<List<Courses>> GetCoursesByInstructorIdAsync(int InstructorId);
        public Task<Courses> UpdateCourse(Courses course, IFormFile File);
        public Task<bool> DeleteCourse(int id);
        Task<List<Courses>> GetAllCoursesPending();
        public Task<int> GetCountByCourseState(int Instructorid, CourseStatusEnum CourseStatusEnum);
        public Task<int> GetCount(int Instructorid);

        Task<bool> UpdateCourseStatus(int id, CourseStatusEnum status, string? reason = null);
        public IQueryable<Courses> GetAllCoursesPaginated();


    }
}

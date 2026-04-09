using LMS.Data_.Entities;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.Abstract
{
    public interface ILessonsService
    {
        public Task<Lessons> Add(Lessons lesson, IFormFile file);
        public Task<bool> NumberOfOrderIsFound(int CourseId, int order);
        public Task<List<Lessons>> GetAllLessonsByCourseId(int CourseId);
        Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber, int? lessonId);
        public Task<Lessons> Update(Lessons lesson, IFormFile? file);
        Task<int> GetCountOfLessonsByCourseId(int courseId);
        Task<bool> DeleteLesson(int Id);
        Task<Lessons> GetLessonsById(int Id);

        Task UpdateWithoutFile(Lessons lesson);
        Task<int> GetTotalLessonsByInstructorId(int InstructorId);




    }
}

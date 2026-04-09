using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface ILessonsRepository : IGenericRepositoryAsync<Lessons>
    {
        Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber);
        public Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber, int? lessonId);

        public Task<List<Lessons>> GetAllLessonsByCourseId(int courseId);

        public Task<int> GetCountOfLessonsByCourseId(int courseId);

        public Task<Lessons> GetLessonById(int Id);

        public Task<int> GetTotalLessonsByInstructorId(int InstructorId);


    }
}

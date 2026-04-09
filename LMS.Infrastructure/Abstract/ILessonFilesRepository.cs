using LMS.Data_.Entities;

namespace LMS.Infrastructure.Abstract
{
    public interface ILessonFilesRepository : IGenericRepositoryAsync<LessonFiles>
    {
        IQueryable<LessonFiles> GetAllByLessonId(int lessonId);
        //   Task<LessonFiles> AddFileAsync(LessonFiles lessonFiles);
        IQueryable<LessonFiles> GetAllMyLessonsFilesAsInstructor(int InstructorId);
        public Task<LessonFiles> GetByIdAsync(int Id);





    }
}

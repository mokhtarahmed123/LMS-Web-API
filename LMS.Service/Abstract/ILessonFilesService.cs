using LMS.Data_.Entities;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.Abstract
{
    public interface ILessonFilesService
    {
        Task<LessonFiles> Add(LessonFiles lessonFiles, IFormFile file);
        Task<LessonFiles> Update(LessonFiles lessonFiles, IFormFile? file);
        Task<LessonFiles> UpdateWithOutFile(LessonFiles lessonFiles);
        Task Delete(int Id);
        IQueryable<LessonFiles> GetAllMyLessonsFilesAsInstructor(int InstructorId);
        IQueryable<LessonFiles> GetAllLessonsFileByLessonsId(int LessonId);

        Task DeleteAllFileByLessonId(int LessonId);

        Task<LessonFiles> GetByIdAsync(int Id);



    }
}

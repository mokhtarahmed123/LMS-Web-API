using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.implementation
{
    public class LessonFilesService : ILessonFilesService
    {
        private readonly ILessonFilesRepository lessonFilesRepository;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment env;

        public LessonFilesService(ILessonFilesRepository lessonFilesRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this.lessonFilesRepository = lessonFilesRepository;
            this.fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;
        }


        public async Task<LessonFiles> Add(LessonFiles lessonFiles, IFormFile file)
        {
            if (file == null)
                throw new ArgumentException("File is required");

            var request = httpContextAccessor.HttpContext?.Request
                ?? throw new InvalidOperationException("No HTTP context");

            var baseUrl = $"{request.Scheme}://{request.Host}";

            var savedEntity = await lessonFilesRepository.AddAsync(lessonFiles);

            try
            {
                var location = $"LessonFiles/Lesson/{savedEntity.LessonId}/File/{savedEntity.Id}";

                var filePath = await fileService.UploadFile(location, file);

                savedEntity.FileUrl = baseUrl + filePath;

                await lessonFilesRepository.UpdateAsync(savedEntity);

                return savedEntity;
            }
            catch (Exception)
            {

                await lessonFilesRepository.DeleteAsync(savedEntity);

                throw;
            }
        }
        public async Task Delete(int Id)
        {

            using var transaction = lessonFilesRepository.BeginTransaction();
            try
            {
                var lesson = await lessonFilesRepository.GetByIdAsync(Id);
                if (lesson == null)
                    throw new NullReferenceException("Lesson Is Null");

                if (!string.IsNullOrEmpty(lesson.FileUrl))
                {
                    var relativePath = lesson.FileUrl.TrimStart('/');
                    var path = Path.Combine(env.WebRootPath, relativePath);
                    await fileService.DeleteFileAsync(path);
                }

                await lessonFilesRepository.DeleteAsync(lesson);
                await lessonFilesRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAllFileByLessonId(int LessonId)
        {
            using var transaction = lessonFilesRepository.BeginTransaction();
            try
            {
                var lessons = lessonFilesRepository.GetAllByLessonId(LessonId);
                if (lessons == null)
                    throw new NullReferenceException("Lesson Is Null ");


                foreach (var lesson in lessons)
                    if (!string.IsNullOrEmpty(lesson.FileUrl))
                    {
                        var relativePath = lesson.FileUrl.TrimStart('/');
                        var path = Path.Combine(env.WebRootPath, relativePath);
                        await fileService.DeleteFileAsync(path);
                    }

                await lessonFilesRepository.DeleteRangeAsync((ICollection<LessonFiles>)lessons);
                transaction.Commit();

            }
            catch
            {
                transaction.Rollback();
                throw;
            }



        }

        public IQueryable<LessonFiles> GetAllLessonsFileByLessonsId(int LessonId)
        {
            return lessonFilesRepository.GetAllByLessonId(LessonId);
        }

        public IQueryable<LessonFiles> GetAllMyLessonsFilesAsInstructor(int InstructorId)
        {
            return lessonFilesRepository.GetAllMyLessonsFilesAsInstructor(InstructorId);
        }

        public async Task<LessonFiles> GetByIdAsync(int Id)
        {
            return await lessonFilesRepository.GetByIdAsync(Id);

        }

        public async Task<LessonFiles> Update(LessonFiles lessonFiles, IFormFile? file)
        {
            if (file != null)
            {
                var request = httpContextAccessor.HttpContext?.Request
                    ?? throw new InvalidOperationException("No HTTP context");

                var baseUrl = $"{request.Scheme}://{request.Host}";
                var location = $"LessonFiles/Lesson/{lessonFiles.LessonId}/File/{lessonFiles.Id}";
                var filepath = await fileService.UploadFile(location, file);

                lessonFiles.FileUrl = baseUrl + filepath;
            }

            await lessonFilesRepository.UpdateAsync(lessonFiles);
            return lessonFiles;

        }

        public async Task<LessonFiles> UpdateWithOutFile(LessonFiles lessonFiles)
        {
            await lessonFilesRepository.UpdateAsync(lessonFiles);

            return lessonFiles;

        }
    }
}

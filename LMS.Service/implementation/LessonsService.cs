using LMS.Data_.Entities;
using LMS.Infrastructure.Abstract;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.implementation
{
    public class LessonsService : ILessonsService
    {
        private readonly ILessonsRepository lessonsRepository;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment env;

        public LessonsService(ILessonsRepository lessonsRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            this.lessonsRepository = lessonsRepository;
            this.fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
            this.env = env;
        }
        public async Task<Lessons> Add(Lessons lesson, IFormFile file)
        {
            if (file == null)
                throw new ArgumentException("Video file is required");

            var request = httpContextAccessor.HttpContext?.Request
                ?? throw new InvalidOperationException("No HTTP context");

            var baseUrl = $"{request.Scheme}://{request.Host}";


            var location = $"videos/courses/{lesson.CourseId}/lessons/{lesson.Id}";

            var videoPath = await fileService.UploadVideo(location, file);

            lesson.VideoUrl = baseUrl + videoPath;

            return await lessonsRepository.AddAsync(lesson);
        }

        public async Task<bool> DeleteLesson(int Id)
        {
            using var transaction = lessonsRepository.BeginTransaction();
            try
            {
                var lesson = await lessonsRepository.GetByIdAsync(Id);
                if (lesson == null)
                    return false;

                if (!string.IsNullOrEmpty(lesson.VideoUrl))
                {
                    var relativePath = lesson.VideoUrl.TrimStart('/');
                    var path = Path.Combine(env.WebRootPath, relativePath);
                    await fileService.DeleteFileAsync(path);
                }

                await lessonsRepository.DeleteAsync(lesson);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Lessons>> GetAllLessonsByCourseId(int CourseId)
        {
            return await lessonsRepository.GetAllLessonsByCourseId(CourseId);

        }

        public async Task<int> GetCountOfLessonsByCourseId(int courseId)
        {
            return await lessonsRepository.GetCountOfLessonsByCourseId(courseId);
        }

        public async Task<Lessons> GetLessonsById(int Id)
        {
            return await lessonsRepository.GetLessonById(Id);
        }

        public async Task<int> GetTotalLessonsByInstructorId(int InstructorId)
        {
            return await lessonsRepository.GetTotalLessonsByInstructorId(InstructorId);
        }

        public async Task<bool> NumberOfOrderIsFound(int CourseId, int order)
        {
            return await lessonsRepository.OrderNumberExistsAsync(CourseId, order);

        }

        public async Task<bool> OrderNumberExistsAsync(int courseId, int orderNumber, int? lessonId)
        {
            return await lessonsRepository.OrderNumberExistsAsync(courseId, orderNumber, lessonId);
        }

        public async Task<Lessons> Update(Lessons lesson, IFormFile? file)
        {
            if (file != null)
            {
                var request = httpContextAccessor.HttpContext?.Request
                    ?? throw new InvalidOperationException("No HTTP context");

                var baseUrl = $"{request.Scheme}://{request.Host}";
                var location = $"courses/{lesson.CourseId}/lessons/{lesson.Id}";
                var videoPath = await fileService.UploadVideo(location, file);

                lesson.VideoUrl = baseUrl + videoPath;
            }

            await lessonsRepository.UpdateAsync(lesson);
            return lesson;
        }
        public async Task UpdateWithoutFile(Lessons lesson)
        {

            await lessonsRepository.UpdateAsync(lesson);
        }
    }
}

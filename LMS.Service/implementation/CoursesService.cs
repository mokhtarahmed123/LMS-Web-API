using LMS.Data_.Entities;
using LMS.Data_.Enum;
using LMS.Infrastructure.Abstract;
using LMS.Infrastructure.Context;
using LMS.Service.Abstract;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.implementation
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository coursesRepository;
        private readonly IFileService fileService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext appDbContext;

        public CoursesService(ICoursesRepository coursesRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor, AppDbContext appDbContext)
        {
            this.coursesRepository = coursesRepository;
            this.fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
            this.appDbContext = appDbContext;
        }

        public async Task<Courses> AddCourse(Courses course, IFormFile File)
        {
            var cotext = httpContextAccessor.HttpContext.Request;
            var baseurl = cotext.Scheme + "://" + cotext.Host;
            var ImageUrl = await fileService.UploadImage("Courses", File);
            course.ThumbnailUrl = baseurl + ImageUrl;

            var Result = await coursesRepository.AddAsync(course);
            return Result;

        }

        public async Task<bool> DeleteCourse(int Id)
        {
            using var transaction = coursesRepository.BeginTransaction();
            try
            {
                var Course = await coursesRepository.GetByIdAsync(Id);

                if (Course == null)
                    return false;
                if (!string.IsNullOrEmpty(Course.ThumbnailUrl))
                {
                    await fileService.DeleteFileAsync(Course.ThumbnailUrl);
                }

                await coursesRepository.DeleteAsync(Course);
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Courses>> GetAllCoursesAsync()
        {
            return await coursesRepository.GetAllCoursesAsync();
        }

        public IQueryable<Courses> GetAllCoursesPaginated()
        {
            return coursesRepository.GetTableNoTracking().AsQueryable();
        }

        public async Task<List<Courses>> GetAllCoursesPending()
        {
            return await coursesRepository.GetAllCoursesPending();
        }

        public async Task<int> GetCount(int Instructorid)
        {
            return await coursesRepository.GetCount(Instructorid);
        }



        public async Task<int> GetCountByCourseState(int Instructorid, CourseStatusEnum CourseStatusEnum)
        {
            return await coursesRepository.GetCountByCourseState(Instructorid, CourseStatusEnum);
        }

        public async Task<Courses> GetCourseById(int id)
        {
            return await coursesRepository.GetCourseByIdAsync(id);
        }

        public async Task<List<Courses>> GetCoursesByCategoryIdAsync(int CategoryId)
        {
            return await coursesRepository.GetCoursesByCategoryIdAsync(CategoryId);
        }

        public async Task<List<Courses>> GetCoursesByInstructorIdAsync(int InstructorId)
        {
            return await coursesRepository.GetCoursesByInstructorIdAsync(InstructorId);
        }

        public async Task<Courses> UpdateCourse(Courses course, IFormFile? file)
        {
            if (file != null)
            {
                var request = httpContextAccessor.HttpContext?.Request
                    ?? throw new InvalidOperationException("No HTTP context");

                var baseUrl = $"{request.Scheme}://{request.Host}";

                var imageUrl = await fileService.UploadImage("Courses", file);

                course.ThumbnailUrl = baseUrl + imageUrl;
            }

            await coursesRepository.UpdateAsync(course);
            return course;
        }

        public async Task<bool> UpdateCourseStatus(int id, CourseStatusEnum status, string? reason = null)
        {
            var course = await GetCourseById(id);
            if (course == null) return false;

            course.CourseStatus = status;
            course.ReasonOfReject = reason;
            course.UpdatedAt = DateTime.UtcNow;

            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}

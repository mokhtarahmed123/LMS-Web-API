using LMS.Data_.Enum;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;

namespace LMS.Service.BackgroundJobs.Courses
{
    public class CourseJobService : ICourseJobService
    {
        private readonly ICoursesService _coursesService;
        private readonly IEmailService _emailService;
        private const int MaxPendingDays = 7; // غيرها لو عايز

        public CourseJobService(ICoursesService coursesService, IEmailService emailService)
        {
            _coursesService = coursesService;
            _emailService = emailService;
        }

        public async Task AutoRejectLongPendingCourses()
        {
            var now = DateTime.UtcNow;

            var pendingCourses = await _coursesService.GetAllCoursesPending();

            var toReject = pendingCourses
                .Where(c => (now - c.CreatedAt).TotalDays > MaxPendingDays)
                .ToList();

            foreach (var course in toReject)
            {
                await _coursesService.UpdateCourseStatus(
                    course.Id,
                    CourseStatusEnum.Rejected,
                    "تم الرفض التلقائي بسبب تجاوز مدة المراجعة"
                );

                // بعت email للـ instructor
                await _emailService.SendEmailAsync(
                    course.InstructorProfile.User.Email,
                    $"تم رفض الكورس '{course.Title}' تلقائياً بسبب تجاوز مدة المراجعة المحددة بـ {MaxPendingDays} أيام.",
                    "رفض الكورس تلقائياً"
                );
            }
        }
    }
}

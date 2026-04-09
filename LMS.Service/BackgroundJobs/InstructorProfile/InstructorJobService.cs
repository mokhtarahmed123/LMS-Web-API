using LMS.Data_.Enum;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;

namespace LMS.Service.BackgroundJobs.InstructorProfile
{
    public class InstructorJobService : IInstructorJobService
    {
        private readonly IInstructorProfilesService _instructorService;
        private readonly IEmailService _emailService;
        private const int MaxPendingDays = 7;

        public InstructorJobService(IInstructorProfilesService instructorService, IEmailService emailService)
        {
            _instructorService = instructorService;
            _emailService = emailService;
        }

        public async Task AutoRejectLongPendingRequests()
        {
            var now = DateTime.UtcNow;

            var pendingInstructors = await _instructorService
                .GetAllInstructorsFilter(StatusOfInstructorProfileEnum.Pending);

            var toReject = pendingInstructors
                .Where(i => (now - i.CreatedAt).TotalDays > MaxPendingDays)
                .ToList();

            foreach (var instructor in toReject)
            {
                instructor.StatusOfInstructorProfile = StatusOfInstructorProfileEnum.Rejected;
                instructor.ReasonOfReject = "تم الرفض التلقائي بسبب تجاوز مدة المراجعة";

                await _instructorService.Update(instructor, "تم الرفض التلقائي بسبب تجاوز مدة المراجعة");

                await _emailService.SendEmailAsync(
                    instructor.User.Email,
                    $"تم رفض طلبك كمدرب تلقائياً بسبب تجاوز مدة المراجعة المحددة بـ {MaxPendingDays} أيام. يمكنك التقديم مرة أخرى.",
                    "رفض طلب المدرب تلقائياً"
                );
            }
        }
    }
}

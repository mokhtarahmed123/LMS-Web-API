using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.UserCourses.Command.Handler
{
    public class DecreaseCourseEnrollmentCountHandler : INotificationHandler<DecreaseCourseEnrollmentNotification>
    {
        private readonly ICoursesService _coursesService;
        public DecreaseCourseEnrollmentCountHandler(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }
        public async Task Handle(DecreaseCourseEnrollmentNotification notification, CancellationToken cancellationToken)
        {
            var course = await _coursesService.GetCourseById(notification.CourseId);
            if (course != null && course.NumberOfEnrolledStudents > 0)
            {
                course.NumberOfEnrolledStudents -= 1;
                await _coursesService.UpdateCourse(course, null);
            }
        }
    }

    public class DecreaseCourseEnrollmentNotification : INotification
    {
        public int CourseId { get; set; }
        public DecreaseCourseEnrollmentNotification(int courseId)
        {
            CourseId = courseId;
        }
    }
}

using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.UserCourses.Command.Handler
{
    public class IncreaseCourseEnrollmentCountHandler : INotificationHandler<IncreaseCourseEnrollmentNotification>
    {
        private readonly ICoursesService _coursesService;

        public IncreaseCourseEnrollmentCountHandler(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }

        public async Task Handle(IncreaseCourseEnrollmentNotification notification, CancellationToken cancellationToken)
        {
            var course = await _coursesService.GetCourseById(notification.CourseId);
            if (course != null)
            {
                course.NumberOfEnrolledStudents += 1;
                await _coursesService.UpdateCourse(course, null);
            }
        }
    }

    public class IncreaseCourseEnrollmentNotification : INotification
    {
        public int CourseId { get; set; }

        public IncreaseCourseEnrollmentNotification(int courseId)
        {
            CourseId = courseId;
        }
    }
}

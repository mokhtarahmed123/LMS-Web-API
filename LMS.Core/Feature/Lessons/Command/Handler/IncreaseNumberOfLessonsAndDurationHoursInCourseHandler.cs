using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.Lessons.Command.Handler
{
    internal class IncreaseNumberOfLessonsAndDurationHoursInCourseHandler : INotificationHandler<IncreaseNumberOfLessonsAndDurationHoursInCourse>
    {
        private readonly ICoursesService _coursesService;
        public IncreaseNumberOfLessonsAndDurationHoursInCourseHandler(ICoursesService coursesService)
        {
            _coursesService = coursesService;
        }
        public async Task Handle(IncreaseNumberOfLessonsAndDurationHoursInCourse notification, CancellationToken cancellationToken)
        {
            var course = await _coursesService.GetCourseById(notification.CourseId);
            if (course != null)
            {
                course.NumberOfLessons += 1;

                course.DurationHours = Math.Round(course.DurationHours + (notification.DurationMinutes / 60m), 2);
                await _coursesService.UpdateCourse(course, null);
            }
        }
    }
    public class IncreaseNumberOfLessonsAndDurationHoursInCourse : INotification
    {
        public int CourseId { get; set; }
        public decimal DurationMinutes { get; set; }
        public IncreaseNumberOfLessonsAndDurationHoursInCourse(int courseId, decimal durationMinutes)
        {
            CourseId = courseId;
            this.DurationMinutes = durationMinutes;

        }
    }
}

using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.Lessons.Command.Handler
{
    public class DecreaseNumberOfLessonsAndDurationHoursInCourseHandler : INotificationHandler<DecreaseNumberOfAndDurationHoursLessonsInCourse>
    {
        private readonly ICoursesService coursesService;

        public DecreaseNumberOfLessonsAndDurationHoursInCourseHandler(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
        }
        public async Task Handle(DecreaseNumberOfAndDurationHoursLessonsInCourse notification, CancellationToken cancellationToken)
        {
            var course = await coursesService.GetCourseById(notification.CourseId);
            if (course != null)
            {

                course.NumberOfLessons = Math.Max(0, course.NumberOfLessons - 1);
                course.DurationHours = Math.Max(0,
                    Math.Round(course.DurationHours - (notification.DurationMinutes / 60m), 2)
                );
                await coursesService.UpdateCourse(course, null);
            }
        }
    }
    public class DecreaseNumberOfAndDurationHoursLessonsInCourse : INotification
    {
        public int CourseId { get; set; }
        public decimal DurationMinutes { get; set; }
        public DecreaseNumberOfAndDurationHoursLessonsInCourse(int courseId, decimal durationMinutes)
        {
            this.CourseId = courseId;
            this.DurationMinutes = durationMinutes;



        }

    }

}


using LMS.Service.Abstract;
using MediatR;

namespace LMS.Core.Feature.UserCourses.Command.Handler
{
    public class RateCourseEnrollmentHandler : INotificationHandler<RateCourseEnrollmentNotification>
    {
        private readonly ICoursesService _coursesService;
        private readonly IUserCoursesService userCoursesService;

        public RateCourseEnrollmentHandler(ICoursesService coursesService, IUserCoursesService userCoursesService)
        {
            _coursesService = coursesService;
            this.userCoursesService = userCoursesService;
        }


        public async Task Handle(RateCourseEnrollmentNotification notification, CancellationToken cancellationToken)
        {
            var course = await _coursesService.GetCourseById(notification.CourseId);
            if (course == null)
                return;

            if (notification.Rate < 1 || notification.Rate > 5)
                return;


            //      var SumOfrating = await userCoursesService.GetSumOfRatingByCourseId(notification.CourseId);
            ///    var CountOfRating = await userCoursesService.GetCountOfRatingByCourseId(notification.CourseId);
            //   var NewAverageRating = (double)SumOfrating / CountOfRating;
            course.AverageRating = (decimal)await userCoursesService.GetAverageRatingByCourseId(notification.CourseId);




            await _coursesService.UpdateCourse(course, null);
        }
    }

    public class RateCourseEnrollmentNotification : INotification
    {

        public int CourseId { get; set; }
        public int Rate { get; set; }

        public RateCourseEnrollmentNotification(int CourseId, int rate)
        {
            this.CourseId = CourseId;
            Rate = rate;
        }
    }
}

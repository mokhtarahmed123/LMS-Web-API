namespace LMS.Service.BackgroundJobs.Courses
{
    public interface ICourseJobService
    {
        Task AutoRejectLongPendingCourses();
    }
}

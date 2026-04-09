namespace LMS.Service.BackgroundJobs.InstructorProfile
{
    public interface IInstructorJobService
    {
        Task AutoRejectLongPendingRequests();
    }
}

namespace LMS.Service.BackgroundJobs.Quiz
{
    public interface IQuizJobService
    {
        Task CheckExpiredQuizzes();

    }
}

using LMS.Service.Abstract.QuizzesAbstract;
using Microsoft.EntityFrameworkCore;

namespace LMS.Service.BackgroundJobs.Quiz
{
    public class QuizJobService : IQuizJobService
    {
        private readonly IQuizService quizService;

        public QuizJobService(IQuizService quizService)
        {
            this.quizService = quizService;
        }

        public async Task CheckExpiredQuizzes()
        {
            var now = DateTime.UtcNow;

            var toDeactivate = quizService.GetAllQuizzesAsync()
                .Where(q => q.IsTimeBound &&
                            q.EndDate.HasValue &&
                            q.EndDate < now);

            foreach (var quiz in await toDeactivate.ToListAsync())
            {
                quiz.IsTimeBound = false;
                await quizService.UpdateQuizAsync(quiz);
            }
        }
    }
}

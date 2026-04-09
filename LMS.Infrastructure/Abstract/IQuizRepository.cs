using LMS.Data_.Entities.Quiz;

namespace LMS.Infrastructure.Abstract
{
    public interface IQuizRepository : IGenericRepositoryAsync<Quizzes>
    {
        public Task<List<Quizzes>> GetAllQuizzesByCourseIdAsync(int courseId);


    }
}

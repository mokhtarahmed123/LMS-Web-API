using LMS.Data_.Entities.Quiz;

namespace LMS.Service.Abstract.QuizzesAbstract
{
    public interface IQuestionOptionsService
    {
        Task<QuestionOptions> GetOptionByIdAsync(int optionId);
        Task<List<QuestionOptions>> GetOptionsByQuestionIdAsync(int questionId);
        Task<QuestionOptions> AddOptionAsync(QuestionOptions option);
        Task<QuestionOptions> UpdateOptionAsync(QuestionOptions option);
        Task<bool> DeleteOptionAsync(int optionId);
        Task<List<QuestionOptions>> GetCorrectAnswersByQuestionIdsAsync(List<int> questionIds);


    }
}

namespace LMS.Core.Feature.QuestionOptions.Query.Result
{
    public class GetQuestionOptionsByQuestionIdResult
    {
        public int NumberOfOption { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }


    }
}

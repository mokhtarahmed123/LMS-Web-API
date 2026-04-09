namespace LMS.Core.Feature.QuizSubmissions.Query.Result
{
    public class GetAllMySubmissionsResult
    {
        public string Email { get; set; }
        public int Score { get; set; }
        public int CourseId { get; set; }
        public int CorrectAnswered { get; set; }
        public int SelectedOption { get; set; }
    }
}

namespace LMS.Core.Feature.Quizzes.Query.Result
{
    public class GetAllQuizzesByCourseIdResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TotalMarks { get; set; }
        public int PassingScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsTimeBound { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}

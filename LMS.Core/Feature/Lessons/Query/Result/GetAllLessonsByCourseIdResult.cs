namespace LMS.Core.Feature.Lessons.Query.Result
{
    public class GetAllLessonsByCourseIdResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Duration { get; set; }
        public string VideoUrl { get; set; }
        public int OrderVideo { get; set; }
        public bool IsFree { get; set; }
        public string CourseName { get; set; }

        public int NumberOfFiles { get; set; }
    }

}

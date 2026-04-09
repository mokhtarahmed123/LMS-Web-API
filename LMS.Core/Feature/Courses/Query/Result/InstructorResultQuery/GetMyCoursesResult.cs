namespace LMS.Core.Feature.Courses.Query.Result.InstructorResultQuery
{
    public class GetMyCoursesResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal DurationHours { get; set; }

        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        //public DateOnly UpdatedAt { get; set; }
        public string CategoryName { get; set; }
        public decimal AverageRating { get; set; }
        public int NumberOfEnrolledStudents { get; set; }
        public string CourseStatus { get; set; }
        public string Level { get; set; }
        public string Language { get; set; }
        public string? ReasonOfRejected { get; set; }

        public int NumberOfLessons { get; set; }
        public List<StudentsResult> Students { get; set; }






    }
    public class StudentsResult
    {
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }

    }
}

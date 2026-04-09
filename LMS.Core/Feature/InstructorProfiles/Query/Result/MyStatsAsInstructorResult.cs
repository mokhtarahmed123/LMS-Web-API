namespace LMS.Core.Feature.InstructorProfiles.Query.Result
{
    public class MyStatsAsInstructorResult
    {
        public string Id { get; set; }
        public int TotalCourses { get; set; }
        public int approvedCourses { get; set; }
        public int pendingCourses { get; set; }
        public int rejectedCourses { get; set; }
        public int totalStudentsEnrolled { get; set; }
        public int totalLessons { get; set; }
        public decimal AverageRate { get; set; }
        public int totalRatings { get; set; }


        public int rating { get; set; }


    }
}

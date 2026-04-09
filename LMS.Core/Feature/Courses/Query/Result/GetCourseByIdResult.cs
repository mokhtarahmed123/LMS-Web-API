using LMS.Data_.Enum;

namespace LMS.Core.Feature.Courses.Query.Result
{
    public class GetCourseByIdResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public int DurationHours { get; set; }

        public CoursesLevelEnum Level { get; set; }
        public CoursesLanguageEnum CourseLanguage { get; set; }
        public string ThumbnailUrl { get; set; }
        public int NumberOfEnrolledStudents { get; set; }
        public int NumberOfLessons { get; set; }

        public decimal AverageRating { get; set; }
        public string CategoryName { get; set; }
        public CourseStatusEnum CourseStatus { get; set; }


    }
}

using LMS.Data_.Enum;

namespace LMS.Core.Feature.UserCourses.Query.Result
{
    public class GetAllMyCoursesEnrollmentsResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationHours { get; set; }
        public bool IsFree { get; set; }
        public CoursesLevelEnum Level { get; set; }
        public CoursesLanguageEnum CourseLanguage { get; set; }
        public string ThumbnailUrl { get; set; }
        public int NumberOfLessons { get; set; }
        public decimal AverageRating { get; set; }
        public string NameOfInstructor { get; set; }
        public bool IsFavourite { get; set; }
        public int? UserRating { get; set; }
        public DateTime EnrolledAt { get; set; }
        public string CategoryName { get; set; }

    }
}

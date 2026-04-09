using LMS.Data_.Enum;

namespace LMS.Core.Feature.Courses.Query.Result.AdminResultQuery
{
    public class GetAllCoursesPendingResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public int DurationHours { get; set; }

        public CoursesLevelEnum Level { get; set; }
        public CoursesLanguageEnum CourseLanguage { get; set; }
        public string ThumbnailUrl { get; set; }
        public string CategoryName { get; set; }

        public int InstructorId { get; set; }

    }
}

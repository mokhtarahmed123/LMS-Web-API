using LMS.Data_.Enum;

namespace LMS.Core.Feature.Courses.Query.Result
{
    public class GetAllCoursesPaginatedResult
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public decimal DurationHours { get; set; }

        public CoursesLevelEnum Level { get; set; }
        public CoursesLanguageEnum CourseLanguage { get; set; }
        public string ThumbnailUrl { get; set; }
        public int NumberOfEnrolledStudents { get; set; }
        public int NumberOfLessons { get; set; }

        public decimal AverageRating { get; set; }
        public string CategoryName { get; set; }
        public CourseStatusEnum CourseStatus { get; set; }

        public GetAllCoursesPaginatedResult(
          string title,
          string description,
          string instructorName,
          string instructorEmail,
          decimal durationHours,

          CoursesLevelEnum level,
          CoursesLanguageEnum courseLanguage,
          string thumbnailUrl,
          int numberOfEnrolledStudents,
          int numberOfLessons,
          decimal averageRating,
          string categoryName,
          CourseStatusEnum courseStatus)
        {
            Title = title;
            Description = description;
            InstructorName = instructorName;
            InstructorEmail = instructorEmail;
            DurationHours = durationHours;

            Level = level;
            CourseLanguage = courseLanguage;
            ThumbnailUrl = thumbnailUrl;
            NumberOfEnrolledStudents = numberOfEnrolledStudents;
            NumberOfLessons = numberOfLessons;
            AverageRating = averageRating;
            CategoryName = categoryName;
            CourseStatus = courseStatus;
        }
    }
}

using LMS.Core.Feature.Courses.Query.Result.InstructorResultQuery;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile
    {
        public void GetMyCoursesAsInstructor()
        {

            CreateMap<Courses, GetMyCoursesResult>()

              .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
              .ForMember(dest => dest.ReasonOfRejected, opt => opt.MapFrom(src => src.ReasonOfReject))
             .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
             //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt.))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.NumberOfLessons, opt => opt.MapFrom(src => src.NumberOfLessons))
             .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.CourseLanguage.ToString()))
             .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating))
             .ForMember(dest => dest.CourseStatus, opt => opt.MapFrom(src => src.CourseStatus.ToString()))
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))

             .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
             .ForMember(dest => dest.NumberOfLessons, opt => opt.MapFrom(src => src.NumberOfLessons))
             .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ThumbnailUrl) ? "/images/default-course.png" : src.ThumbnailUrl))
             .ForMember(dest => dest.NumberOfEnrolledStudents, opt => opt.MapFrom(src => src.NumberOfEnrolledStudents));



        }
    }
}

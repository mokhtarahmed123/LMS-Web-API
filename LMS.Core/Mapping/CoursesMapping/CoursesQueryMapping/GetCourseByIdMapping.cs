using LMS.Core.Feature.Courses.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile
    {
        public void GetById()
        {
            CreateMap<Courses, GetCourseByIdResult>()
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
             .ForMember(dest => dest.NumberOfLessons, opt => opt.MapFrom(src => src.NumberOfLessons))
             .ForMember(dest => dest.CourseLanguage, opt => opt.MapFrom(src => src.CourseLanguage.ToString()))
             .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.AverageRating))
                  .ForMember(dest => dest.CourseStatus, opt => opt.MapFrom(src => src.CourseStatus.ToString()))

             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
             .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.InstructorProfile.User.Email))
             .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.InstructorProfile.User.UserName))

             .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
             .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ThumbnailUrl) ? "/images/default-course.png" : src.ThumbnailUrl))
             .ForMember(dest => dest.NumberOfEnrolledStudents, opt => opt.MapFrom(src => src.NumberOfEnrolledStudents));



        }
    }
}

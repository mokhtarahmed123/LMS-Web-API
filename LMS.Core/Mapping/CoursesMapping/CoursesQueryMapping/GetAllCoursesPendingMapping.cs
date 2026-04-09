using LMS.Core.Feature.Courses.Query.Result.AdminResultQuery;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile
    {
        public void GetAllCoursesPending()
        {

            CreateMap<Courses, GetAllCoursesPendingResult>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))

                 .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorProfileId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                 .ForMember(dest => dest.CourseLanguage, opt => opt.MapFrom(src => src.CourseLanguage.ToString()))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ThumbnailUrl) ? "/images/default-course.png" : src.ThumbnailUrl));

        }
    }
}

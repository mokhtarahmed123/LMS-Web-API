using LMS.Core.Feature.UserCourses.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.UserCoursesMapping
{
    public partial class UserCoursesProfile
    {
        public void GetAllCoursesFavourite()
        {
            CreateMap<UserCourses, GetMyFavouriteCoursesByUserIdResult>()
        .ForMember(dest => dest.NameOfInstructor, opt =>
       opt.MapFrom(src =>
         src.Course.InstructorProfile != null &&
        src.Course.InstructorProfile.User != null
           ? src.Course.InstructorProfile.User.UserName
         : string.Empty)).ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.Course.AverageRating))
     .ForMember(dest => dest.IsFavourite, opt => opt.MapFrom(src => src.IsFavorite))
     .ForMember(dest => dest.UserRating, opt => opt.MapFrom(src => src.Rating))
     .ForMember(dest => dest.EnrolledAt, opt => opt.MapFrom(src => src.EnrolledAt))
     .ForMember(dest => dest.DurationHours, opt => opt.MapFrom(src => src.Course.DurationHours))

     .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Course.ThumbnailUrl))
     .ForMember(dest => dest.CourseLanguage, opt => opt.MapFrom(src => src.Course.CourseLanguage))
     .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Course.Level))
     .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Course.Description))
     .ForMember(dest => dest.NumberOfLessons, opt => opt.MapFrom(src => src.Course.NumberOfLessons))
     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Course.Id))
     .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Course.Category.Name))


     ;

        }
    }
}

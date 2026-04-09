using LMS.Core.Feature.Courses.Command.Models.InstructorCommand;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile
    {
        public void AddCourse()
        {

            CreateMap<AddCourseCommand, Courses>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                //  .ForMember(dest => dest.InstructorProfileId, opt => opt.MapFrom(src => src.InstructorProfileId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.DurationHours, opt => opt.MapFrom(src => src.DurationHours))

                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                .ForMember(dest => dest.CourseLanguage, opt => opt.MapFrom(src => src.CourseLanguage.ToString()));
            //  .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailUrl))
        }
    }
}

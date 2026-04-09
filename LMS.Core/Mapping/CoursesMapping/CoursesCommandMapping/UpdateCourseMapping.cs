using LMS.Core.Feature.Courses.Command.Models.InstructorCommand;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile
    {
        public void Update()
        {

            CreateMap<UpdateCourseCommand, Courses>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CourseLanguage, opt => opt.MapFrom(src => src.CourseLanguage))
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorProfileId, opt => opt.Ignore())

                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(Courses => Courses.NumberOfEnrolledStudents, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));





        }
    }
}

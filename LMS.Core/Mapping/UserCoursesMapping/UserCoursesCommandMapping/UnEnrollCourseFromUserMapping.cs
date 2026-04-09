using LMS.Core.Feature.UserCourses.Command.Models.Student;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.UserCoursesMapping
{
    public partial class UserCoursesProfile
    {
        public void UnEnroll()
        {
            CreateMap<UnenrollCourseCommand, UserCourses>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
               ;


        }
    }
}

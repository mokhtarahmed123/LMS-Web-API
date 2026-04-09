using LMS.Core.Feature.UserCourses.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.UserCoursesMapping
{
    public partial class UserCoursesProfile
    {
        public void GetAllStudentByCourseIdMapping()
        {
            CreateMap<UserCourses, GetAllStudentByCourseIdResult>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.EnrollAt, opt => opt.MapFrom(src => src.EnrolledAt))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
                 .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.User.UserName))

                  .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        }
    }
}

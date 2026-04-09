using AutoMapper;

namespace LMS.Core.Mapping.UserCoursesMapping
{
    public partial class UserCoursesProfile : Profile
    {
        public UserCoursesProfile()
        {
            Enroll();
            UnEnroll();
            GetAllStudentByCourseIdMapping();
            GetAllCoursesEnrollmentsByUserId();
            GetAllCoursesFavourite();
        }
    }
}

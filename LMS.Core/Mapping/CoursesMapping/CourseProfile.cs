using AutoMapper;

namespace LMS.Core.Mapping.CoursesMapping
{
    public partial class CourseProfile : Profile
    {
        public CourseProfile()
        {
            AddCourse();
            GetAll();
            GetById();
            GetAllByCategoryId();
            GetAllByInstructorId();
            Update();
            GetMyCoursesAsInstructor();
            GetAllCoursesPending();
        }
    }
}

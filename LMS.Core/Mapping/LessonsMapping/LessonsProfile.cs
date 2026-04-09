using AutoMapper;

namespace LMS.Core.Mapping.LessonsMapping
{
    public partial class LessonsProfile : Profile
    {
        public LessonsProfile()
        {
            Add();
            Update();
            GetAllLessonsByCourseId();
            GetLessonById();
        }
    }
}

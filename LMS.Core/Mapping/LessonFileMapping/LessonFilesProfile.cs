using AutoMapper;

namespace LMS.Core.Mapping.LessonFileMapping
{
    public partial class LessonFilesProfile : Profile
    {
        public LessonFilesProfile()
        {
            Add();
            GetAll();
            GetById();
            Edit();
        }
    }
}

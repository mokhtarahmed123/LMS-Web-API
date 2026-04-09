using LMS.Core.Feature.LessonFiles.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.LessonFileMapping
{
    public partial class LessonFilesProfile
    {
        private void Edit()
        {
            CreateMap<EditLessonsFileCommand, LessonFiles>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId));
        }
    }
}

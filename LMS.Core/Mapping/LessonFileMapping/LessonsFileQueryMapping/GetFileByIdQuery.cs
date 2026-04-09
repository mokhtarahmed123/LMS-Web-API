using LMS.Core.Feature.LessonFiles.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.LessonFileMapping
{
    public partial class LessonFilesProfile
    {
        private void GetById()
        {
            CreateMap<LessonFiles, GetFileByIdResult>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FileUrl))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.Title))
                .ForMember(dest => dest.LessonNumber, opt => opt.MapFrom(src => src.LessonId))
                .ForMember(dest => dest.UploadAt, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));
        }
    }
}

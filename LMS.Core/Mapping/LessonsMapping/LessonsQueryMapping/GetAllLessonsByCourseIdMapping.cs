using LMS.Core.Feature.Lessons.Query.Result;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.LessonsMapping
{
    public partial class LessonsProfile
    {
        public void GetAllLessonsByCourseId()
        {

            CreateMap<Lessons, GetAllLessonsByCourseIdResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.OrderVideo, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Title))
                .ForMember(dest => dest.IsFree, opt => opt.MapFrom(src => src.IsPreview))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl))
                .ForMember(dest => dest.NumberOfFiles, opt => opt.MapFrom(src => src.NumberOfFiles))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.DurationMinutes));



        }



    }
}

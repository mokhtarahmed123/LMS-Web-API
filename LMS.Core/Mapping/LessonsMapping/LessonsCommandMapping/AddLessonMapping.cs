using LMS.Core.Feature.Lessons.Command.Models;
using LMS.Data_.Entities;

namespace LMS.Core.Mapping.LessonsMapping
{
    public partial class LessonsProfile
    {
        public void Add()
        {

            CreateMap<AddLessonCommand, Lessons>()
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.DurationMinutes))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.IsPreview, opt => opt.MapFrom(src => src.IsPreview));


        }
    }
}

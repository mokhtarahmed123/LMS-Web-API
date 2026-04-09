using LMS.Core.Feature.Quizzes.Query.Result;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.QuizzesMapping
{
    public partial class QuizzesProfile
    {
        private void GetAllQuizzesByCourseid()
        {
            CreateMap<Quizzes, GetAllQuizzesByCourseIdResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TotalMarks, opt => opt.MapFrom(src => src.TotalMarks))
                .ForMember(dest => dest.IsTimeBound, opt => opt.MapFrom(src => src.IsTimeBound))

                .ForMember(dest => dest.PassingScore, opt => opt.MapFrom(src => src.PassingScore))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
        }
    }
}

using LMS.Core.Feature.QuizSubmissions.Query.Result;
using LMS.Data_.Entities.Quiz;

namespace LMS.Core.Mapping.Submissions
{
    public partial class SubmissionsProfile
    {
        private void GetAllMySubmissions()
        {
            CreateMap<SubmissionAnswers, GetAllMySubmissionsResult>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Submission.Student.Email))
                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.Submission.Quiz.CourseId))
                .ForMember(dest => dest.SelectedOption, opt => opt.MapFrom(src => src.SelectedOptionId))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Submission.Score))
                .ForMember(dest => dest.CorrectAnswered, opt => opt.MapFrom(src =>
                    src.SelectedOption != null && src.SelectedOption.IsCorrect ? 1 : 0)); // ✅
        }
    }
}

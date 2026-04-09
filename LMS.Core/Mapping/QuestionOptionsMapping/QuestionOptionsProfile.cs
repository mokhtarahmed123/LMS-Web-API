using AutoMapper;

namespace LMS.Core.Mapping.QuestionOptionsMapping
{
    public partial class QuestionOptionsProfile : Profile
    {
        public QuestionOptionsProfile()
        {
            Add();
            Update();
            GetQuestionOptionsByQuestionId();
            GetQuestionOptionsById();
        }
    }
}

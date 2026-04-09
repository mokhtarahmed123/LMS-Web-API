using AutoMapper;

namespace LMS.Core.Mapping.Submissions
{
    public partial class SubmissionsProfile : Profile
    {
        public SubmissionsProfile()
        {
            GetAllMySubmissions();
        }
    }
}

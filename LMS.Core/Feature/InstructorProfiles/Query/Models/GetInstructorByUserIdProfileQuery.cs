using LMS.Core.Feature.InstructorProfiles.Query.Result;
using MediatR;

namespace LMS.Core.Feature.InstructorProfiles.Query.Models
{
    public record GetInstructorByUserIdProfileQuery(string UserId) : IRequest<Response<GetInstructorByUserIdProfileResult>>;
}

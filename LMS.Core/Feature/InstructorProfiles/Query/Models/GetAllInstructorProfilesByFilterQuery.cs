using LMS.Core.Feature.InstructorProfiles.Query.Result;
using LMS.Data_.Enum;
using MediatR;

namespace LMS.Core.Feature.InstructorProfiles.Query.Models
{
    public record GetAllInstructorProfilesByFilterQuery(StatusOfInstructorProfileEnum? statusOfInstructorProfileEnum) : IRequest<Response<List<GetAllInstructorProfilesByFilterResult>>>
   ;
}

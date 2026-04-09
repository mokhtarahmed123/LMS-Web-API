using LMS.Core.Feature.Authorization.Query.Result;
using MediatR;

namespace LMS.Core.Feature.Authorization.Query.Models
{
    public record GetUsersByRoleNameQuery(string RoleName) : IRequest<Response<GetUsersByRoleNameResult>>;


}

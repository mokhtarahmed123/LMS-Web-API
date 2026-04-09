using MediatR;

namespace LMS.Core.Feature.Emails.Query.Models
{
    public record ConfirmEmailQuery(string Code, string UserId) : IRequest<Response<string>>;

}

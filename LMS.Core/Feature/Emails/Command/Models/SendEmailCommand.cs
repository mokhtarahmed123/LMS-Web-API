using MediatR;

namespace LMS.Core.Feature.Emails.Command.Models
{
    public record SendEmailCommand(string Email, string Massege) : IRequest<Response<string>>;

}

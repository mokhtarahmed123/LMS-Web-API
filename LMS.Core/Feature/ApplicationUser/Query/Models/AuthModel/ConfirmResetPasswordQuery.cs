using MediatR;

namespace LMS.Core.Feature.ApplicationUser.Query.Models.AuthModel
{
    public record ConfirmResetPasswordQuery(string Code, string Email) : IRequest<Response<string>>;

}

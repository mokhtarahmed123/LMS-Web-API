using LMS.Core.Bases;
using LMS.Core.Feature.Emails.Command.Models;
using LMS.Core.Resources;
using LMS.Service.EmailServices;
using MediatR;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Emails.Command.Handler
{
    public class EmailCommandHandler : ResponseHandler, IRequestHandler<SendEmailCommand, Response<string>>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IEmailService emailService;

        public EmailCommandHandler(IStringLocalizer<SharedResources> stringLocalizer, IEmailService emailService) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.emailService = emailService;
        }
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await emailService.SendEmailAsync(request.Email, request.Massege, null);
            if (response == "Success")
                return Success<string>("");
            return BadRequest<string>(stringLocalizer[SharedResourcesKeys.SendEmailFailed]);

        }
    }
}

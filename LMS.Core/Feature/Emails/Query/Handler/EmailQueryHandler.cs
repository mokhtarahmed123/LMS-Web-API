using LMS.Core.Bases;
using LMS.Core.Resources;
using LMS.Service.Abstract;
using LMS.Service.EmailServices;
using Microsoft.Extensions.Localization;

namespace LMS.Core.Feature.Emails.Query.Handler
{
    public class EmailQueryHandler : ResponseHandler
    {


        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IEmailService emailService;
        private readonly IAuthService authService;

        public EmailQueryHandler(IStringLocalizer<SharedResources> stringLocalizer, IEmailService emailService, IAuthService authService) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.emailService = emailService;
            this.authService = authService;
        }




    }
}

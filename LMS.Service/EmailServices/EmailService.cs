using LMS.Data_.Helper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LMS.Service.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<string> SendEmailAsync(string email, string messageText, string? reason)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));


                message.To.Add(MailboxAddress.Parse(email));

                message.Subject = reason ?? "Notification";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = messageText,
                    TextBody = "Welcome to LMS"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();


                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await client.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.SmtpPort,
                    SecureSocketOptions.StartTls
                );

                await client.AuthenticateAsync(
                    _emailSettings.Username,
                    _emailSettings.Password
                );

                await client.SendAsync(message);

                await client.DisconnectAsync(true);

                return "Success";
            }
            catch (Exception ex)
            {
                return $"Failed: {ex.Message}";
            }
        }
    }
}
namespace LMS.Service.EmailServices
{
    public interface IEmailService
    {
        public Task<string> SendEmailAsync(string email, string Massage, string? reason);
    }
}

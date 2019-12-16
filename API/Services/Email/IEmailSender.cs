using System.Threading.Tasks;

namespace API.Services.Email
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string from, string to, string subject, string message, string htmlMessage);
    }
}
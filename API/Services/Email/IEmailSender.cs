using System.Threading.Tasks;

namespace API.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string from, string to, string subject, string message, string htmlMessage);
    }
}
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace API.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string from, string to, string subject, string message, string htmlMessage)
        {
            var client = new SendGridClient(Constants.Email.ApiKey);
            var sendGridMessage = new SendGridMessage
            {
                From = new EmailAddress(from, Constants.Email.User),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = htmlMessage
            };
            
            sendGridMessage.AddTo(to);
            await client.SendEmailAsync(sendGridMessage);
        }
    }
}
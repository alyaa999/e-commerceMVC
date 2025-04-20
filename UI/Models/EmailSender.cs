using e_commerce.Web.Models;
using System.Net.Mail;
using System.Net;
using e_commerce.Application.Common.Interfaces;

namespace e_commerce.Web.Models
{
    public class EmailSender : IEmailSenderService
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IConfiguration configuration)
        {
            _emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.Port,
                Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password),
                EnableSsl = _emailSettings.EnableSSL
            };

            var message = new MailMessage(_emailSettings.FromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}



using Clothify.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;


namespace Clothify.Infrastructure.AuthenticationServices
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSetting;
        public EmailService(SmtpSettings smtpSettings)
        {
            _smtpSetting = smtpSettings;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string htmlContent, string plainTextContent)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(_smtpSetting.SenderName, _smtpSetting.Username));
                mimeMessage.To.Add(MailboxAddress.Parse(email));
                mimeMessage.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = htmlContent,
                    TextBody = plainTextContent
                };
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSetting.SenderEmail, _smtpSetting.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);

                return true; // success
            }
            catch (Exception)
            {
                return false; // failed
            }
        }
    }
}

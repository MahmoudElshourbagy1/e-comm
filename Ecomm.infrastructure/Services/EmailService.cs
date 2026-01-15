using Ecom.Core.DTO;
using Ecom.Core.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;


namespace Ecomm.infrastructure.Services
{
    public class EmailService : IEmailService
    {
        //SMTP
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }
        public async Task sendEmail(EmailDTO emailDTO)
        {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("my Ecom",config["EmailSetting:From"]));
            message.Subject=emailDTO.Subject;
            message.To.Add(new MailboxAddress(emailDTO.To,emailDTO.To));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = emailDTO.Content
            };
            using (var smtp = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(config["EmailSetting:smtp"],
                       int.Parse(config["EmailSetting:Port"]),
                       true
                        );
                    await smtp.AuthenticateAsync(config["EmailSetting:UserName"],
                        config["EmailSetting:Password"]
                        );
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Email send failed: {ex.Message}");
                }
                finally
                {
                    smtp.Disconnect(true);
                    //smtp.Dispose();
                }
            }
        }
    }
}

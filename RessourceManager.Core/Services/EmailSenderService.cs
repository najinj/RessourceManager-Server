using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IBackOfficeSettingsService _backOfficeSettingsService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmailSenderService(IBackOfficeSettingsService backOfficeSettingsService, IHostingEnvironment hostingEnvironment)
        {
            _backOfficeSettingsService = backOfficeSettingsService;
            _hostingEnvironment = hostingEnvironment;
        }

        public Task SendActivationEmailAsync(string email)
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Templates/ActivationEmail.html");
            var body = File.ReadAllText(path);
            return SendEmailAsync(email, "Your Account has been Activated", body);
        }

        public Task SendResetPasswordEmailAsync(string email,string token)
        {
            var path = Path.Combine(_hostingEnvironment.ContentRootPath, "Templates/ResetPasswordEmail.html");
            var body = File.ReadAllText(path);
            body = body.Replace("#token#", token);
            return SendEmailAsync(email, "Reset Password", body);
        }

        private async Task SendEmailAsync(string email, string subject,string body)
        {
            try
            {
                var backOfficeSettings = await _backOfficeSettingsService.Get();

                var emailSettings = backOfficeSettings.EmailSettings;

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress("naji.ensat@gmail.com"));

                mimeMessage.Subject = subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = body,
                };

                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(emailSettings.MailServer, emailSettings.MailPort);
                    
                    await client.AuthenticateAsync(emailSettings.Sender, emailSettings.Password);

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }

            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}

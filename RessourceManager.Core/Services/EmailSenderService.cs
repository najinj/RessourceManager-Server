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
        private readonly IEmailSettingService _emailSettingService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public EmailSenderService(IEmailSettingService emailSettingService,IHostingEnvironment hostingEnvironment)
        {
            _emailSettingService = emailSettingService;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task SendEmailAsync(string email, string subject)
        {
            try
            {
                var emailSettings = await _emailSettingService.Get();

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(emailSettings.SenderName, emailSettings.Sender));

                mimeMessage.To.Add(new MailboxAddress("naji.ensat@gmail.com"));

                mimeMessage.Subject = subject;

                string Body = Path.Combine(_hostingEnvironment.ContentRootPath, "Templates/ActivationEmail.html");




                mimeMessage.Body = new TextPart("html")
                {
                    Text = File.ReadAllText(Body),
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

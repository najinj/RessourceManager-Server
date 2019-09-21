using MimeKit;
using MongoDB.Driver;
using RessourceManagerApi.Models;
using System;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using test_mongo_auth.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RessourceManagerApi.Services
{
    public class EmailSenderService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EmailSenderService(IRessourceDatabaseSettings settings, IHostingEnvironment hostingEnvironment)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _emailSettings = database.GetCollection<EmailSettings>(settings.EmailSettings).Find(emailSettings=>true).FirstOrDefault();
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task SendEmailAsync(string email, string subject)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));

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
                    

                    /*if (_env.IsDevelopment())
                    {
                        // The third parameter is useSSL (true if the client should make an SSL-wrapped
                        // connection to the server; otherwise, false).
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, true);
                    }
                    else
                    {
                    */
                        await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort);
                    // }

                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync(_emailSettings.Sender, _emailSettings.Password);

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

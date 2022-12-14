using BloodBankApp.Areas.HospitalAdmin.Model;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class EmailSenderService : IEmail
    {
        EmailSettings _emailSettings = null;
        public EmailSenderService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }
        public async Task<bool> SendEmail(EmailData emailData)
        {
            try
            {
                var emailMessage = new MimeMessage();
                var emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                var emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;
                var emailBodyBuilder = new BodyBuilder
                {
                    TextBody = emailData.EmailBody
                };
                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                var emailClient = new SmtpClient();
                await emailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, false);
                await emailClient.AuthenticateAsync(_emailSettings.EmailId, _emailSettings.Password);
                await emailClient.SendAsync(emailMessage);
                await emailClient.DisconnectAsync(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                //Log Exception Details
                return false;
            }
        }
    }
}

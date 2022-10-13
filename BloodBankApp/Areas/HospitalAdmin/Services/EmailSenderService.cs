using BloodBankApp.Areas.HospitalAdmin.Model;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services {
    public class EmailSenderService : IEmail {
       EmailSettings _emailSettings = null;
    public EmailSenderService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }
    public bool SendEmail(EmailData emailData)
    {
        try
        {
            MimeMessage emailMessage = new MimeMessage();
            MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
            emailMessage.To.Add(emailTo);
            emailMessage.Subject = emailData.EmailSubject;
            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = emailData.EmailBody;
            emailMessage.Body = emailBodyBuilder.ToMessageBody();
            SmtpClient emailClient = new SmtpClient();
            emailClient.Connect(_emailSettings.Host, _emailSettings.Port, false);
            emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
            emailClient.Send(emailMessage);
            emailClient.Disconnect(true);
            emailClient.Dispose();
            return true;
        }
        catch(Exception ex)
        {
            //Log Exception Details
            return false;
        }
    }
    }
}

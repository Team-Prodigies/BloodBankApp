using BloodBankApp.Areas.HospitalAdmin.Model;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers {
    [Area("HospitalAdmin")]
    [Authorize(Roles = "HospitalAdmin")]
    public class SendEmailController : Controller {

        IEmail _emailService = null;
        public SendEmailController(IEmail emailService) {
            _emailService = emailService;
        }

        public IActionResult SendMail() {
            var emailData = new EmailData {
                EmailBody = "Congrats you donated 3 times",
                EmailToName = "Egzon",
                EmailToId = "egzonbllaca88@gmail.com",
                EmailSubject = "Free Check Up"
            };
            _emailService.SendEmail(emailData);
            return View();
        }

    }
}

using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize(Roles = "HospitalAdmin")]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public MessagesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult ChatRoom()
        {
            var currentHospitalAdminId = _userManager.GetUserId(User);
            var hospitalAdmin = _context.MedicalStaffs.Where(ms => ms.MedicalStaffId == new Guid(currentHospitalAdminId)).FirstOrDefault();
            var hospital = _context.Hospitals.Where(h => h.HospitalId == hospitalAdmin.HospitalId).FirstOrDefault();
            ViewBag.HospitalId = hospital.HospitalId;

            return View();
        }
    }
}

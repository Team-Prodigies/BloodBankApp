using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IHospitalService _hospitalService;
        private readonly UserManager<User> _userManager;
        public MessagesController(IHospitalService hospitalService,
            UserManager<User> userManager)
        {
            _hospitalService = hospitalService;
            _userManager = userManager;
        }

        [Authorize(Policy = Permissions.HospitalAdmin.MessageDonors)]
        public async Task<IActionResult> ChatRoom()
        {
            var currentHospitalAdminId = _userManager.GetUserId(User);
            var hospital = await _hospitalService.GetHospitalForMedicalStaff(currentHospitalAdminId);
            ViewBag.HospitalId = hospital.HospitalId;
            return View();
        }
    }
}

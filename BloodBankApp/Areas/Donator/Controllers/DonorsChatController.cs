using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize]
    public class DonorsChatController : Controller
    {
        private readonly IHospitalService _hospitalService;
        private readonly UserManager<User> _userManager;
        public DonorsChatController(IHospitalService hospitalService, UserManager<User> userManager)
        {
            _hospitalService = hospitalService;
            _userManager = userManager;
        }

        [Authorize(Policy = Permissions.Donors.ViewHospitalChatRooms)]
        public async Task<IActionResult> Hospitals(int pageNumber = 1)
        {
            var hospitals = await _hospitalService.GetHospitals(pageNumber);
            ViewBag.PageNumber = pageNumber;
            return View(hospitals);
        }

        [Authorize(Policy = Permissions.Donors.SendMessageToHospital)]
        public async Task<IActionResult> DonorChatRoomAsync(Guid hospitalId)
        {
            ViewBag.DonorId = _userManager.GetUserId(User);
            var hospital = await _hospitalService.GetHospital(hospitalId);
            return View(hospital);
        }
    }
}

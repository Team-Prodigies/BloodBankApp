using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize(Roles = "Donor")]
    public class DonorsChatController : Controller
    {
        private readonly IHospitalService _hospitalService;

        public DonorsChatController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        public async Task<IActionResult> Hospitals(int pageNumber=1)
        {
            var hospitals = await _hospitalService.GetHospitals(pageNumber);
            ViewBag.PageNumber = pageNumber;
            return View(hospitals);
        }
        public async Task<IActionResult> HospitalChatRoomAsync(Guid hospitalId)
        {
            var hospitalAdmins = await _hospitalService.GetAllHospitalAdminsByHospitalId(hospitalId);      
            var hospital = await _hospitalService.GetHospital(hospitalId);

            ViewBag.HospitalId = hospitalId;
            ViewBag.HospitalName = hospital.HospitalName;

            return View(hospitalAdmins);
        }
    }
}

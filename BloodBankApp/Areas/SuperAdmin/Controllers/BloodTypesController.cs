using BloodBankApp.Areas.SuperAdmin.Services;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class BloodTypesController : Controller
    {
        private readonly IBloodTypesService _bloodTypesService;

        public BloodTypesController(IBloodTypesService bloodTypesService)
        {
            _bloodTypesService = bloodTypesService;
        }

        public IActionResult CreateBloodType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBloodType(string bloodTypeName)
        {
            await _bloodTypesService.AddNewBloodType(bloodTypeName);
            return RedirectToAction(nameof(BloodTypes));
        }

        [HttpPost]
        public async Task<IActionResult> EditBloodType(BloodType editBloodType)
        {
            await _bloodTypesService.EditBloodType(editBloodType);
            return RedirectToAction(nameof(BloodTypes));
        }

        [HttpGet]
        public async Task<IActionResult> EditBloodType(Guid bloodTypeID)
        {
            var bloodType = await _bloodTypesService.GetBloodType(bloodTypeID);
            if (bloodType == null)
            {
                return RedirectToAction(nameof(BloodTypes));
            }
            return View(bloodType);
        }

        public async Task<IActionResult> BloodTypes()
        {
            var bloodTypes = await _bloodTypesService.GetAllBloodTypes();
            return View(bloodTypes);
        }
    }
}

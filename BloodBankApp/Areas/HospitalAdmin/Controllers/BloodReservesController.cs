using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
    public class BloodReservesController : Controller
    {
        private readonly IBloodReservesService _bloodReservesService;
        private readonly IBloodTypesService _bloodTypesService;
        private readonly INotyfService _notyfService;

        public BloodReservesController(IBloodReservesService bloodReservesService,
            INotyfService notyfService,
            IBloodTypesService bloodTypesService)
        {
            _bloodReservesService = bloodReservesService;
            _notyfService = notyfService;
            _bloodTypesService = bloodTypesService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.ViewBloodReserves)]

        public async Task<IActionResult> Index()
        {
            var reserves = await _bloodReservesService.GetBloodReserves();
            return View(reserves);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.SetBloodReserves)]
        public async Task<IActionResult> AddReserve(Guid reserveId)
        {
            ViewBag.BloodTypes = new SelectList(await _bloodTypesService.GetAllBloodTypes(), "BloodTypeId", "BloodTypeName");
            if (reserveId == Guid.Empty)
                return View();

            var model = await _bloodReservesService.GetBloodReserve(reserveId);
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.SetBloodReserves)]
        public async Task<IActionResult> AddReserve(BloodReserveModel model)
        {
            ViewBag.BloodTypes = new SelectList(await _bloodTypesService.GetAllBloodTypes(), "BloodTypeId", "BloodTypeName");
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _bloodReservesService.SetBloodReserve(model);
            if (result)
            {
                _notyfService.Success("Blood reserve set successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Success("Couldn't set blood reserve");
            return View();
        }
    }
}
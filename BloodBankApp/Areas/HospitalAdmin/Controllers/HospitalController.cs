using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
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
    public class HospitalController : Controller
    {
        private readonly IHospitalService _hospitalService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly SelectList _cityList;

        public HospitalController(ICitiesService citiesService,
            IHospitalService hospitalService,
            INotyfService notyfService,
            IMapper mapper)
        {
            _hospitalService = hospitalService;
            _notyfService = notyfService;
            _mapper = mapper;
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
        }

        [Authorize(Policy = Permissions.HospitalAdmin.ViewHospital)]
        public async Task<IActionResult> ManageHospital()
        {
            var hospitalAdmin = await _hospitalService.GetHospitalForHospitalAdmin(User);
            ViewData["CityId"] = _cityList;
            return View(hospitalAdmin);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.EditHospital)]
        public async Task<IActionResult> EditHospital(Guid hospitalId)
        {
            var hospital = await _hospitalService.GetHospitalForHospitalAdm(hospitalId);

            if (hospital == null)
            {
                _notyfService.Error("Hospital was not found!");
                return RedirectToAction(nameof(ManageHospital));
            }
            ViewData["CityId"] = _cityList;
            ViewData["Location"] = await _hospitalService.GetAllLocations();

            var editHospital = _mapper.Map<EditHospitalModel>(hospital);
            return View(editHospital);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.EditHospital)]
        public async Task<IActionResult> EditHospital(EditHospitalModel hospital)
        {

            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = _cityList;
                return View(hospital);
            }
            await _hospitalService.EditHospitalForHospitalAdmin(hospital);
            _notyfService.Success("Hospital updated successfully!");
         return RedirectToAction(nameof(EditHospital), new { hospital.HospitalId });
     
        }

    }
}

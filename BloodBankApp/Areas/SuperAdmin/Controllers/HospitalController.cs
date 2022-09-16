using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]

    public class HospitalController : Controller
    {
        private readonly IMapper _mapper;
        private readonly SelectList _cityList;
        private readonly IHospitalService _hospitalService;
        private readonly INotyfService _notyfService;
        public HospitalController(ICitiesService citiesService,
            IMapper mapper,
            IHospitalService hospitalService, INotyfService notyfService)
        {
            _mapper = mapper;
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
            _hospitalService = hospitalService;
            _notyfService = notyfService;
        }

        [Authorize(Policy = Permissions.Hospitals.Create)]
        public IActionResult CreateHospital()
        {
            ViewData["CityId"] = _cityList;
            return View();
        }

        [Authorize(Policy = Permissions.Hospitals.View)]
        public async Task<IActionResult> ManageHospitals(int pageNumber = 1)
        {
            var hospitals = await _hospitalService.GetHospitals(pageNumber);
            ViewBag.pageNumber = pageNumber;
            return View(hospitals);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Hospitals.Create)]
        public async Task<IActionResult> CreateHospital(HospitalModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = _cityList;
                return View();
            }
            var hospitalCodeInUse = await _hospitalService.HospitalCodeExists(model.HospitalCode);
            var result = await _hospitalService.CreateHospital(model);
            
            if (!result || hospitalCodeInUse)
            {
                ViewData["CityId"] = _cityList;
                ViewData["hospitalCodeInUse"] = "This hospital code is already taken!";
                return View();
            }

            _notyfService.Success("Hospital added successfully!");
            return RedirectToAction(nameof(ManageHospitals));
        }


        [HttpGet]
        [Authorize(Policy = Permissions.Hospitals.Edit)]
        public async Task<IActionResult> EditHospital(Guid hospitalId)
        {
            var hospital = await _hospitalService.GetHospital(hospitalId);
           
            if (hospital == null)
            {
                _notyfService.Error("Hospital was not found!");
                return RedirectToAction(nameof(ManageHospitals));
            }
            ViewData["CityId"] = _cityList;
            var editHospital = _mapper.Map<HospitalModel>(hospital);
            return View(editHospital);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Hospitals.Edit)]
        public async Task<IActionResult> EditHospital(HospitalModel hospital)
        {

            if (!ModelState.IsValid) {
                ViewData["CityId"] = _cityList; 
                return View(hospital);
            }

            var oldHospitalCode = await _hospitalService.GetHospitalCode(hospital.HospitalId);         

            if(oldHospitalCode != null && oldHospitalCode != hospital.HospitalCode)
            {
                var hospitalCodeInUse = await _hospitalService.HospitalCodeExists(hospital.HospitalCode);
                if (hospitalCodeInUse)
                {
                    ViewData["hospitalCodeInUse"] = "This hospital code is already taken!";
                    ViewData["CityId"] = _cityList;
                    return View(hospital);
                }
            }
            await _hospitalService.EditHospital(hospital);
            _notyfService.Success("Hospital updated successfully!");
            return RedirectToAction(nameof(EditHospital), new { hospital.HospitalId });
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Hospitals.View)]
        public async Task<IActionResult> HospitalSearchResults(string searchTerm, int pageNumber = 1)
        {
            if (searchTerm == null || searchTerm.Trim() == "") {
                return RedirectToAction(nameof(ManageHospitals));
            }
            var hospitals = await _hospitalService.HospitalSearchResults(searchTerm, pageNumber);
          
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(hospitals);
        }
    }
}

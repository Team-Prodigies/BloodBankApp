using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize(Roles = "HospitalAdmin")]
    public class DonatorsController : Controller
    {
        private readonly IDonatorService _donatorsService;
        private readonly IDonorsService _donorsService;
        private readonly SelectList _cityList;
        private readonly SelectList _bloodTypeList;
        private readonly INotyfService _notyfService;

        public DonatorsController(IDonatorService donatorsService,
            IDonorsService donorsService,
            ICitiesService citiesService,
            IBloodTypesService bloodTypesService,
            INotyfService notyfService)
        {
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
            _bloodTypeList = new SelectList(bloodTypesService.GetAllBloodTypes().Result, "BloodTypeId", "BloodTypeName");
            _donatorsService = donatorsService;
            _donorsService = donorsService;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> ManageDonators()
        {
            var donators = await _donatorsService.GetDonators();
            return View(donators);
        }

        public IActionResult AddDonor()
        {
            ViewData["CityId"] = _cityList;
            ViewData["BloodTypeId"] = _bloodTypeList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDonor(NotRegisteredDonor model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = _cityList;
                ViewData["BloodTypeId"] = _bloodTypeList;
                return View();
            }
            var codeExists =await _donatorsService.CodeExists(model.Code.CodeValue);
            var personalNumberInUse = await _donorsService.PersonalNumberIsInUse(model.PersonalNumber);
           
            if (personalNumberInUse && codeExists)
            {
                ViewData["CityId"] = _cityList;
                ViewData["BloodTypeId"] = _bloodTypeList;
                ViewData["PersonalNumberInUse"] = "This personal number is already taken!";
                ViewData["codeInUse"] = "This code is already taken !!";
                return View();
            }
            else if (personalNumberInUse)
            {
                ViewData["CityId"] = _cityList;
                ViewData["BloodTypeId"] = _bloodTypeList;
                ViewData["PersonalNumberInUse"] = "This personal number is already taken!";
                return View();
            }
            else if (codeExists)
            {
                ViewData["CityId"] = _cityList;
                ViewData["BloodTypeId"] = _bloodTypeList;
                ViewData["codeInUse"] = "This code is already taken !!";
                return View();
            }
            var result = await _donatorsService.AddNotRegisteredDonor(model);
            if (!result)
            {
                ViewData["CityId"] = _cityList;
                ViewData["BloodTypeId"] = _bloodTypeList;
                return View();
            }

            _notyfService.Success("Donor added successfully!");
            return RedirectToAction(nameof(ManageDonators));
        }
    }
}

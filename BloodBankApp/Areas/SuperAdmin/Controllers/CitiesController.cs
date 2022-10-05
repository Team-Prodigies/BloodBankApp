using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.ViewModels;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class CitiesController : Controller
    {
        private readonly ICitiesService _citiesService;
        private readonly INotyfService _notyfService;
        public CitiesController(ICitiesService citiesService,
            INotyfService notyfService)
        {
            _citiesService = citiesService;
            _notyfService = notyfService;
        }

        [Authorize(Policy = Permissions.Cities.View)]
        public async Task<IActionResult> Cities()
        {
            var cities = await _citiesService.GetCities();
            return View(cities.ToList());
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Cities.Create)]
        public async Task<IActionResult> AddNewCity(CityModel city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _citiesService.AddCity(city);
            if (!result)
            {
                _notyfService.Error("City already exists!");
                return View();
            }
            _notyfService.Success("Successfully added city!");
            return RedirectToAction(nameof(Cities));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Cities.Create)]
        public IActionResult AddNewCity()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Cities.Edit)]
        public async Task<IActionResult> EditCity(Guid cityId, City city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (cityId != city.CityId)
            {
                _notyfService.Warning("City was not found!");
                return View();
            }
            var result = await _citiesService.EditCity(city.CityId, city.CityName);
            if (!result)
            {
                _notyfService.Error("City name already exists!");
                return View();
            }
            _notyfService.Success("City was updated!");
            return RedirectToAction(nameof(Cities));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Cities.Edit)]
        public async Task<IActionResult> EditCity(Guid cityId)
        {
            var editCity = await _citiesService.GetCity(cityId);
            if (editCity == null)
            {
                _notyfService.Warning("City was not found!");
                return RedirectToAction(nameof(Cities));
            }
            return View(editCity);
        }
    }
}
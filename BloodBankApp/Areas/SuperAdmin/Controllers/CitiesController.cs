using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class CitiesController : Controller
    {
        private readonly ICitiesService _citiesService;

        public CitiesController(ICitiesService citiesService)
        {
            _citiesService = citiesService;
        }

        public async Task<IActionResult> Cities()
        {
            var cities = await _citiesService.GetCities();
            return View(cities);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _citiesService.AddCity(city);
            return RedirectToAction(nameof(Cities));
        }

        [HttpGet]
        public IActionResult AddNewCity()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditCity(Guid cityId, City city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (cityId != city.CityId)
            {
                return NotFound();
            }
            await _citiesService.EditCity(city.CityId, city.CityName);
            return RedirectToAction(nameof(Cities));
        }

        [HttpGet]
        public async Task<IActionResult> EditCity(Guid cityID)
        {
            var editCity = await _citiesService.GetCity(cityID);
            if (editCity == null)
            {
                return RedirectToAction(nameof(Cities));
            }
            return View(editCity);
        }
    }
}

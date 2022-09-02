using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            return View(cities.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddNewCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _citiesService.AddCity(city);
            if (!result)
            {
                ModelState.AddModelError("", "This City already exists");
                return View();
            }
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
            var result = await _citiesService.EditCity(city.CityId, city.CityName);
            if (!result)
            {
                ModelState.AddModelError("", "This City already exists");
                return View();
            }
            return RedirectToAction(nameof(Cities));
        }

        [HttpGet]
        public async Task<IActionResult> EditCity(Guid cityId)
        {
            var editCity = await _citiesService.GetCity(cityId);
            if (editCity == null)
            {
                return RedirectToAction(nameof(Cities));
            }
            return View(editCity);
        }
    }
}

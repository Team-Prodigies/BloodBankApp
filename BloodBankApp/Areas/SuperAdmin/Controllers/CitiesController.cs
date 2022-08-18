using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Cities()
        {
            var Cities = _context.Cities.ToList();

            return View(Cities);
        }

        [HttpPost]
        public IActionResult AddNewCity(City city) 
        {
            if (ModelState.IsValid)
            {
                _context.Cities.Add(city);

                _context.SaveChanges();

                return RedirectToAction("Cities");
            }
            return RedirectToAction("Cities");
        }

        [HttpGet]
        public IActionResult AddNewCity()
        {
            return View();
        }


        [HttpPost]
        public IActionResult EditCity(Guid cityId, String cityName)
        {
            var cityExists = _context.Cities.Where(b => b.CityName.ToUpper() == cityName.ToUpper()).FirstOrDefault();
            if (cityExists == null)
            {
                var city = _context.Cities.Find(cityId);
                if (city != null)
                {
                    city.CityName = cityName;
                    _context.Update(city);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Cities");
        }

        [HttpGet]
        public IActionResult EditCity(Guid cityID)
        {
            var editCity = _context.Cities.Find(cityID);

            if (editCity == null)
            {
                return RedirectToAction("Cities");
            }

            return View(editCity);
        }
    }
}

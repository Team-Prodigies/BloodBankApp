using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;

namespace BloodBankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IHospitalService _hospitalService;

        public HomeController(IStatisticsService statisticsService, IHospitalService hospitalService)
        {
            _statisticsService = statisticsService;
            _hospitalService = hospitalService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["UsersCount"] = await _statisticsService.GetUsersCountAsync();
            ViewData["HospitalCount"] =await _statisticsService.GetHospitalsCountAsync();
            ViewData["AmountOfDonations"] = await _statisticsService.GetAmountOfBloodDonatedAsync();
            ViewData["Location"] = await _hospitalService.GetAllLocations();
            ViewData["GetAllHospitals"] = await _hospitalService.GetAllHospitals();
            return View();
        }

        public IActionResult DonationProcess()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult DonationBenefits()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BloodBankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService _statisticsService;

        public HomeController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            ViewData["UsersCount"] = _statisticsService.GetUsersCountAsync();
            ViewData["HospitalCount"] = _statisticsService.GetUsersCountAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

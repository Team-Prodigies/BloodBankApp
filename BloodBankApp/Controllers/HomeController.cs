using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BloodBankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService _statisticsService;
        
        public HomeController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["UsersCount"] = await _statisticsService.GetUsersCountAsync();
            ViewData["HospitalCount"] =await _statisticsService.GetHospitalsCountAsync();
            ViewData["AmountOfDonations"] = await _statisticsService.GetAmountOfBloodDonatedAsync();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
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

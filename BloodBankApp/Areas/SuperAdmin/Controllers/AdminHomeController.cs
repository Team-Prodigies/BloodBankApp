using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminHomeController : Controller
    {

        private readonly IStatisticsService _statisticsService;
        private readonly INotyfService _notyfService;
        public AdminHomeController(IStatisticsService statisticsService, INotyfService notyfService)
        {
            _statisticsService = statisticsService;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {           
            ViewData["UserRolesData"] = await _statisticsService.GetUserRoleDataAsync();
            ViewData["BloodTypeData"] = await _statisticsService.GetUserBloodDataAsync();
            ViewData["DonorCount"] = await _statisticsService.GetDonorCountAsync();
            ViewData["BloodAmount"] = await _statisticsService.GetAmountOfBloodDonatedAsync();
            ViewData["DonationPostsCount"] = await _statisticsService.GetNumberOfDonationPostsAsync();
            return View();
        }
    }
}

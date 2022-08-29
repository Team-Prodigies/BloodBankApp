using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminHomeController : Controller
    {

        private readonly IStatisticsService _statisticsService;

        public AdminHomeController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
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

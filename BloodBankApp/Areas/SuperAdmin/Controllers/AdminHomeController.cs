using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BloodBankApp.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class AdminHomeController : Controller
    {

        private readonly IStatisticsService _statisticsService;
        public AdminHomeController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        [Authorize(Policy = Permissions.SuperAdmin.ViewStatistics)]
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class AdminHomeController : Controller
    {

        private readonly IStatisticsService _statisticsService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;

        public AdminHomeController(IStatisticsService statisticsService,
            INotificationService notificationService,
            UserManager<User> userManager)
        {
            _statisticsService = statisticsService;
            _notificationService = notificationService;
            _userManager = userManager;
        }
        [Authorize(Policy = Permissions.SuperAdmin.ViewStatistics)]
        public async Task<IActionResult> Index()
        {           
            ViewData["UserRolesData"] = await _statisticsService.GetUserRoleDataAsync();
            ViewData["BloodTypeData"] = await _statisticsService.GetUserBloodDataAsync();
            ViewData["DonorCount"] = await _statisticsService.GetDonorCountAsync();
            ViewData["BloodAmount"] = await _statisticsService.GetAmountOfBloodDonatedAsync();
            ViewData["DonationPostsCount"] = await _statisticsService.GetNumberOfDonationPostsAsync();
            var user =  _userManager.GetUserId(User);
            var notifications = await _notificationService.GetNotificationsForUser(user);
            ViewBag.Notifications = notifications;
            return View();
        }
    }
}

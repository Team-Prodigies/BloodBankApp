using BloodBankApp.Areas.SuperAdmin.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Policy = Permissions.Donors.ViewDashboard)]
        public IActionResult Index()
        {
            return View();
        }
    }
}

using BloodBankApp.Areas.SuperAdmin.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize(Policy = Permissions.HospitalAdmin.ViewDashboard)]
        public IActionResult Index()
        {
            return View();
        }
    }
}

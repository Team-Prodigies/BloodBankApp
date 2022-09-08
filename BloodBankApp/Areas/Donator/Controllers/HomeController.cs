using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.Donator.Controllers
{
    public class HomeController : Controller
    {
        [Area("Donator")]
        [Authorize(Roles="Donor")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

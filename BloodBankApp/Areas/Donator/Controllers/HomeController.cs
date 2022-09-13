using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize(Roles = "Donor")]
    public class HomeController : Controller
    {
     
        public IActionResult Index()
        {
            return View();
        }
    }
}

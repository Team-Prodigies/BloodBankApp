using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    public class HomeController : Controller
    {
        [Area("HospitalAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

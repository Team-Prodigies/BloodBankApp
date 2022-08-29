using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    public class HomeController : Controller
    {
        [Area("HospitalAdmin")]
        [Authorize(Roles="HospitalAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

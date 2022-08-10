using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class CitiesController : Controller
    {
        public IActionResult Cities()
        {
            return View();
        }
    }
}

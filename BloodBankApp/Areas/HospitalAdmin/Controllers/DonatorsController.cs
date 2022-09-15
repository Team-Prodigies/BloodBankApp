using BloodBankApp.Areas.HospitalAdmin.Services;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize(Roles = "HospitalAdmin")]
    public class DonatorsController : Controller
    {
        private readonly IDonatorService _donatorsService;

        public DonatorsController(IDonatorService donatorsService)
        {
            _donatorsService = donatorsService;
        }

        public async Task<IActionResult> ManageDonators()
        {
            var donators = await _donatorsService.GetDonators();
            return View(donators);
        }
    }
}

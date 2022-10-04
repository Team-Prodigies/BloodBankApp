using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    public class DonationsController : Controller
    {
        private readonly IDonationsService _donationsService;
        private readonly IDonorsService _donorsService;
        private readonly INotyfService _notyfService;

        public DonationsController(IDonationsService donationsService,
            INotyfService notyfService,
            IDonorsService donorsService)
        {
            _donationsService = donationsService;
            _notyfService = notyfService;
            _donorsService = donorsService;
        }

        public async Task<IActionResult> Index()
        {
            var donations = await _donationsService.GetAllDonations();
            return View(donations);
        }

        public async Task<IActionResult> AddDonation(long? personalNumber)
        {
            if (!personalNumber.HasValue) 
                return View();

            var donor = await _donorsService.FindDonor(personalNumber.Value);
            if (donor == null)
            {
                _notyfService.Error($"Donor with personal number {personalNumber} was not found");
                return View();
            }

            ViewBag.DonorFound = true;
            var model = new BloodDonationModel { Donor = donor };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddDonation(BloodDonationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _donationsService.AddDonation(model);
            if (result)
            {
                _notyfService.Success("Donation added successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error("Something went wrong!");
            ViewBag.DonorFound = true;
            return View(model);
        }
    }
}
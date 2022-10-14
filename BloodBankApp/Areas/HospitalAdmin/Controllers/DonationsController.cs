using System;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
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

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.ViewBloodDonations)]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var donations = await _donationsService.GetAllDonations(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(donations);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.ViewDonationRequests)]
        public async Task<IActionResult> DonationRequests()
        {
            var requests = await _donationsService.GetAllDonationRequests();
            return View(requests);
        }

        [Authorize(Policy = Permissions.HospitalAdmin.ApproveDonationRequests)]
        public async Task<IActionResult> ApproveRequest(Guid requestId, double amount)
        {
            var result = await _donationsService.ApproveDonationRequest(requestId, amount);
            if (result)
            {
                _notyfService.Success("Request approved");
                return RedirectToAction(nameof(DonationRequests));
            }
            _notyfService.Error("Failed to approve request");
            return RedirectToAction(nameof(DonationRequests));
        }

        [Authorize(Policy = Permissions.HospitalAdmin.RejectDonationRequests)]
        public async Task<IActionResult> RejectRequest(Guid requestId)
        {
            var result = await _donationsService.RemoveDonationRequest(requestId);
            if (result)
            {
                _notyfService.Success("Request removed");
                return RedirectToAction(nameof(DonationRequests));
            }
            _notyfService.Error("Failed to remove request");
            return RedirectToAction(nameof(DonationRequests));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.AddBloodDonations)]
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
        [Authorize(Policy = Permissions.HospitalAdmin.AddBloodDonations)]
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

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.UpdateBloodDonations)]
        public async Task<IActionResult> EditDonation(Guid donationId)
        {
            var donation = await _donationsService.GetDonation(donationId);
            return View(donation);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.UpdateBloodDonations)]
        public async Task<IActionResult> EditDonation(BloodDonationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _donationsService.UpdateDonation(model);
            if (result)
            {
                _notyfService.Success("Donation updated successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error("Something went wrong!");
            return View(model);
        }
    }
}
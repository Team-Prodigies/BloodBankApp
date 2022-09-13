using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class DonorsController : Controller
    {
        private readonly IDonorsService _donorsService;
        private readonly IUsersService _usersService;
        private readonly INotyfService _notyfService;
        public DonorsController(IDonorsService donorsService,
            IUsersService usersService, INotyfService notyfService)
        {
            _donorsService = donorsService;
            _usersService = usersService;
            _notyfService = notyfService;
        }
        [HttpGet]
        [Authorize(Policy = Permissions.Donors.View)]
        public async Task<IActionResult> Donors(int pageNumber = 1, string filterBy = "A-Z")
        {
            var donors = await _donorsService.GetDonors(pageNumber, filterBy);
            ViewBag.FilterBy = filterBy;
            ViewBag.PageNumber = pageNumber;
            return View(donors);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Donors.View)]
        public async Task<IActionResult> DonorSearchResults(string searchTerm, int pageNumber = 1)
        {
            if (searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Donors));
            }
            var donors = await _donorsService.DonorSearchResults(searchTerm, pageNumber);

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(donors);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.Donors.Lock)]
        public async Task<IActionResult> LockoutDonor(Guid donorId)
        {
            var donor = await _usersService.GetUser(donorId);
            if (donor == null)
            {
                _notyfService.Warning("Donor was not found!");
                return RedirectToAction(nameof(Donors));
            }
            await _donorsService.LockoutDonor(donor);
            _notyfService.Success("Donor "+donor.UserName+" has been locked out!");
            return RedirectToAction(nameof(Donors));
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Donors.Unlock)]
        public async Task<IActionResult> UnlockDonor(Guid donorId)
        {
            var donor = await _usersService.GetUser(donorId);
            if (donor == null)
            {
                _notyfService.Warning("Donor was not found!");
                return RedirectToAction(nameof(Donors));
            }
            await _donorsService.UnlockDonor(donor);
            _notyfService.Success("Donor " + donor.UserName + " has been unlocekd!");
            return RedirectToAction(nameof(Donors));
        }
    }
}
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class DonorsController : Controller
    {
        private readonly IDonorsService _donorsService;
        private readonly IUsersService _usersService;

        public DonorsController(IDonorsService donorsService,
            IUsersService usersService)
        {   
            _donorsService = donorsService;
            _usersService = usersService;
        }
        [HttpGet]
        public async Task<IActionResult> Donors(int pageNumber = 1, string filterBy = "A-Z")
        {
            var donors = await _donorsService.GetDonors(pageNumber, filterBy);     
            ViewBag.FilterBy = filterBy;
            ViewBag.PageNumber = pageNumber;
            return View(donors);
        }

        [HttpGet]
        public async Task<IActionResult> DonorSearchResults(string searchTerm, int pageNumber = 1)
        {
            if(searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Donors));
            }
            var donors = await _donorsService.DonorSearchResults(searchTerm, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;
            return View(donors);
        }
        [HttpPost]
        public async Task<IActionResult> LockoutDonor(Guid donorId)
        {
            var donor = await _usersService.GetUser(donorId);
            if (donor == null)
            {
                return NotFound();
            }
            await _donorsService.LockoutDonor(donor);   
            return RedirectToAction(nameof(Donors));
        }

        [HttpPost]
        public async Task<IActionResult> UnlockDonor(Guid donorId)
        {
            var donor =  await _usersService.GetUser(donorId);
            if (donor == null)
            {
                return NotFound();
            }
            await _donorsService.UnlockDonor(donor);
            return RedirectToAction(nameof(Donors));
        }
    }
}
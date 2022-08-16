using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class DonorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DonorsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Donors()
        {
            var users = await _context.Donors
                .Select(d => new Donor
                {
                    DonorId = d.DonorId,
                    User = d.User,
                    PersonalNumber = d.PersonalNumber,
                    Gender = d.Gender,
                    BloodType = d.BloodType,
                    City = d.City
                }).ToListAsync();

            var result = _mapper.Map<List<DonorModel>>(users);

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> LockoutDonor(Guid donorId)
        {
            var donor = await _context.Users.FindAsync(donorId);
            if(donor != null)
            {
                donor.LockoutEnabled = true;
                donor.LockoutEnd = DateTime.Now.AddMinutes(10); // DateTime.MaxValue if we want to lock a user forever
                _context.Update(donor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Donors");
        }

        [HttpGet]
        public async Task<IActionResult> DonorLockout(Guid donorId)
        {
            var donorLockout =await _context.Donors.FindAsync(donorId);

            if (donorLockout == null)
            {
                return RedirectToAction("Donors");
            }
            return View(donorLockout);
        }


    }
}

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
        public async Task<IActionResult> Donors(int pageNumber = 1, string filterBy = "A-Z")
        {
            var skipRows = (pageNumber - 1) * 10;
            var donors = new List<Donor>();

            switch (filterBy)
            {
                case "A-Z":
                    donors = await _context.Donors.Include(user => user.User).Include(blood => blood.BloodType).Include(city => city.City).OrderBy(donor => donor.User.Name).Skip(skipRows).Take(10).ToListAsync();
                    break;

                case "Z-A":
                    donors = await _context.Donors.Include(user => user.User).Include(blood => blood.BloodType).Include(city => city.City).OrderByDescending(donor => donor.User.Name).Skip(skipRows).Take(10).ToListAsync();
                    break;

                case "Locked":
                    donors = await _context.Donors.Include(user => user.User).Include(blood => blood.BloodType).Include(city => city.City).Where(donor => donor.User.Locked == true).Skip(skipRows).Take(10).ToListAsync();
                    break;

                default:
                    donors = await _context.Donors.Include(user => user.User).Include(blood => blood.BloodType).Include(city => city.City).OrderBy(donor => donor.User.Name).Skip(skipRows).Take(10).ToListAsync();
                    break;
            }
            
            var result = _mapper.Map<List<DonorModel>>(donors);

            ViewBag.FilterBy = filterBy;
            ViewBag.PageNumber = pageNumber;

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> DonorSearchResults(string searchTerm, int pageNumber = 1)
        {
            if(searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Donors));
            }
            var skipRows = (pageNumber - 1) * 10;
            var donors = await _context.Donors.Include(user => user.User).Include(blood => blood.BloodType).Include(city => city.City).Where(donor => (donor.User.Name + donor.User.Surname.ToUpper()).Contains(searchTerm.Replace(" ", "").ToUpper())).Skip(skipRows).Take(10).ToListAsync();
            var result = _mapper.Map< List<DonorModel>>(donors);

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> LockoutDonor(Guid donorId)
        {
            var donor = await _context.Users.FindAsync(donorId);
            if (donor == null)
            {
                return NotFound();
            }
           donor.Locked = true;
            _context.Update(donor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Donors));
        }

        [HttpGet]
        public async Task<IActionResult> DonorLockout(Guid donorId)
        {
            var donorLockout = await _context.Donors.FindAsync(donorId);

            if (donorLockout == null)
            {
                return NotFound();
            }
            return View(donorLockout);
        }

        [HttpPost]
        public async Task<IActionResult> UnlockDonor(Guid donorId)
        {
            var donor = await _context.Users.FindAsync(donorId);
            if (donor == null)
            {
                return NotFound();
            }
            donor.Locked = false;
            _context.Update(donor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Donors));
        }
    }

}
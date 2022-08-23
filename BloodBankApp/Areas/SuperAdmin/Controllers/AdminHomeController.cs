using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminHomeController : Controller {

        private readonly ApplicationDbContext _context;
        public AdminHomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await GetUserRoleDataAsync();
            await GetUserBloodData();
            await GetDonorCount();
            await GetAmountOfBloodDonated();
            await GetNumberOfDonationPosts();
            return View();
        }

        private async Task GetNumberOfDonationPosts()
        {
            ViewData["DonationPostsCount"] = await _context.DonationPosts.Where(x => x.DateRequired.Year == DateTime.Now.Year).CountAsync();
        }

        private async Task GetAmountOfBloodDonated()
        {
            var bloodAmount = 0.0D;
            await _context.BloodDonations.Where(x => x.DonationDate.Year == DateTime.Now.Year).ForEachAsync(x => bloodAmount += x.Amount);
            ViewData["BloodAmount"] = bloodAmount;
        }

        private async Task GetDonorCount()
        {
            ViewData["DonorCount"] = await _context.BloodDonations.Where(x => x.DonationDate.Year == DateTime.Now.Year).CountAsync();
        }

        private async Task GetUserRoleDataAsync()
        {
            var data = new Dictionary<String, int>();

            var userCountByRole =
                from role in await _context.Roles.ToListAsync()
                join user in await _context.UserRoles.ToListAsync() on role.Id equals user.RoleId into users
                select new
                {
                    Role = role.Name,
                    UserCount = users.Count()
                };

            foreach (var entry in userCountByRole)
            {
                data.Add(entry.Role, entry.UserCount);
            }
            ViewData["UserRolesData"] = data;
        }

        private async Task GetUserBloodData()
        {
            var data = new Dictionary<String, int>();

            var donorCountByBloodType =
                from bloodType in await _context.BloodTypes.ToListAsync()
                join donor in await _context.Donors.ToListAsync() on bloodType.BloodTypeId equals donor.BloodTypeId into donors
                select new
                {
                    BloodType = bloodType.BloodTypeName,
                    DonorCount = donors.Count()
                };

            foreach (var entry in donorCountByBloodType)
            {
                data.Add(entry.BloodType, entry.DonorCount);
            }
            ViewData["BloodTypeData"] = data;
        }
    }
}

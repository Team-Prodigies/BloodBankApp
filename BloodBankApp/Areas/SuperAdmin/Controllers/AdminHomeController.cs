using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class AdminHomeController : Controller
    {

        private readonly ApplicationDbContext context;
        public AdminHomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            await GetUserRoleDataAsync();
            await GetUserBloodData();
            await GetDonorCount();
            await GetAmountOfBloodDonated();
            return View();
        }

        private async Task GetAmountOfBloodDonated()
        {
            var bloodAmount = 0.0D;
            await context.BloodDonations.Where(x => x.DonationDate.Year == DateTime.Now.Year).ForEachAsync(x => bloodAmount += x.Amount);
            ViewData["BloodAmount"] = bloodAmount;
        }

        private async Task GetDonorCount()
        {
            var count = await context.BloodDonations.Where(x => x.DonationDate.Year == DateTime.Now.Year).CountAsync();
            ViewData["DonorCount"] = count;
        }

        private async Task GetUserRoleDataAsync()
        {
            var data = new Dictionary<String, int>();

            var userCountByRole =
                from role in await context.Roles.ToListAsync()
                join user in await context.UserRoles.ToListAsync() on role.Id equals user.RoleId into users
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
                from bloodType in await context.BloodTypes.ToListAsync()
                join donor in await context.Donors.ToListAsync() on bloodType.BloodTypeId equals donor.BloodTypeId into donors
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

        public IActionResult ManageCities()
        {
            return View();
        }
    }
}

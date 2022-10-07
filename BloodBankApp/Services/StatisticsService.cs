using BloodBankApp.Data;
using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationDbContext _context;

        public StatisticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetAmountOfBloodDonatedAsync()
        {
            var bloodDonated = await _context.BloodDonations.CountAsync();

            return bloodDonated;
        }

        public async Task<int> GetDonorCountAsync()
        {
            return await _context.BloodDonations
                 .Where(x => x.DonationDate.Year == DateTime.Now.Year)
                 .CountAsync();
        }

        public async Task<int> GetHospitalsCountAsync()
        {
            return await _context.Hospitals.CountAsync();
        }

        public async Task<int> GetNumberOfDonationPostsAsync()
        {
            return await _context.DonationPosts
                  .Where(x => x.DateRequired.Year == DateTime.Now.Year)
                  .CountAsync();
        }

        public async Task<Dictionary<string, int>> GetUserBloodDataAsync()
        {
            var data = new Dictionary<string, int>();

            var donorCountByBloodType =
                from bloodType in await _context.BloodTypes.ToListAsync()
                join donor in await _context.Donors.ToListAsync()
                on bloodType.BloodTypeId equals donor.BloodTypeId into donors
                select new
                {
                    BloodType = bloodType.BloodTypeName,
                    DonorCount = donors.Count()
                };

            foreach (var entry in donorCountByBloodType)
            {
                data.Add(entry.BloodType, entry.DonorCount);
            }
            return data;
        }

        public async Task<Dictionary<string, int>> GetUserRoleDataAsync()
        {
            var data = new Dictionary<string, int>();

            var userCountByRole =
                from role in await _context.Roles.ToListAsync()
                join user in await _context.UserRoles.ToListAsync()
                on role.Id equals user.RoleId into users
                select new
                {
                    Role = role.Name,
                    UserCount = users.Count()
                };

            foreach (var entry in userCountByRole)
            {
                data.Add(entry.Role, entry.UserCount);
            }
            return data;
        }

        public async Task<int> GetUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

    }
}
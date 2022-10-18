using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System.Security.Claims;
using BloodBankApp.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using BloodBankApp.Data;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class HospitalAdminService : IHospitalAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public HospitalAdminService(
           UserManager<User> userManager,
           IHttpContextAccessor httpContextAccessor,
           ApplicationDbContext context)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<IdentityResult> EditHospitalAdmin(HospitalAdminModel hospitalModel)
        {
            var getUser = await GetUser(_httpContextAccessor.HttpContext.User);
            getUser.Name = hospitalModel.Name;
            getUser.Surname = hospitalModel.Surname;
            getUser.UserName = hospitalModel.UserName;
            getUser.NormalizedUserName = hospitalModel.UserName.ToTitleCase();
            getUser.DateOfBirth = hospitalModel.DateOfBirth;
            getUser.PhoneNumber = hospitalModel.PhoneNumber;

            var result = await _userManager.UpdateAsync(getUser);
            return result;
        }

        public async Task<Guid> GetHospitalIdFromHospitalAdmin(Guid userId)
        {
            var hospitalAdmin = await _context.MedicalStaffs.FindAsync(userId);

            return hospitalAdmin.HospitalId;
        }

        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }
    }
}
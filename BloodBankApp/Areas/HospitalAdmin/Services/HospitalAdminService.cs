using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System.Security.Claims;
using BloodBankApp.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.EntityFrameworkCore;
using BloodBankApp.Data;
using System.Linq;
using BloodBankApp.Areas.Services;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class HospitalAdminService : IHospitalAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly UsersService _userService;

        public HospitalAdminService(
           UserManager<User> userManager,
           IHttpContextAccessor httpContextAccessor,
           ApplicationDbContext context,
           UsersService userService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userService = userService;
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

        //public async Task<City> GetHospitalCity(Guid id)
        //{
        //    //var hospitalId = await _context.MedicalStaffs
        //    //    .Where(hospitalAdmin => hospitalAdmin.MedicalStaffId == _userService.GetUser(_httpContextAccessor.HttpContext.User).Result.Id)
        //    //    .Select(hospitalAdmin => hospitalAdmin.HospitalId).FirstOrDefaultAsync();

        //    //var user = await _userService.GetUser(_httpContextAccessor.HttpContext.User);

        //    //var cityId = await _context.MedicalStaffs
        //    //    .Where(hospitalAdmin => hospitalAdmin.HospitalId == user.Id)
        //    //    .Select(city => );

        //    return City;
        //}

        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }


    }
}

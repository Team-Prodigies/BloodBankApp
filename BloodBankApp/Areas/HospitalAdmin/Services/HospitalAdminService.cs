using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System.Security.Claims;
using BloodBankApp.ExtensionMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class HospitalAdminService : IHospitalAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HospitalAdminService(
           UserManager<User> userManager,
           IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }
    }
}

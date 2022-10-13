using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IHospitalAdminService
    {
        Task<IdentityResult> EditHospitalAdmin(HospitalAdminModel hospitalModel);
        Task<User> GetUser(ClaimsPrincipal principal);
    }
}
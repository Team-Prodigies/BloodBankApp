using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterMedicalStaffModel;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterModel;

namespace BloodBankApp.Areas.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUser(Guid id);
        string GetUserId(ClaimsPrincipal principal);
        Task<User> GetUser(ClaimsPrincipal principal);
        Task<User> GetUserByUsername(string username);
        Task<IdentityResult> AddSuperAdmin(SuperAdminModel user);
        Task<IdentityResult> EditSuperAdmin(User user);
        Task<IdentityResult> AddDonor(RegisterInputModel input);
        Task<IdentityResult> AddHospitalAdmin(RegisterMedicalStaffInputModel input);
        Task<IdentityResult> SetPhoneNumber(User user, string phoneNumber);
        Task<IdentityResult> SetUserName(User user, string username);
        Task<bool> UserIsInRole(User user,string role);
    }
}

using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Pages.Account;
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
        Task<IdentityResult> EditSuperAdmin(ProfileAdminModel user);
        Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword);
        Task<IdentityResult> AddDonor(RegisterInputModel input);
        Task<IdentityResult> AddNonRegisteredDonor(RegisterModel.RegisterInputModel input);
        Task<IdentityResult> AddHospitalAdmin(RegisterMedicalStaffInputModel input);
        Task<bool> UserIsInRole(User user,string role);
        Task<RegisterInputModel> DonorExists(RegisterInputModel input);
        Task<List<ManageUserModel>> GetUsers(string roleFilter, int pageNumber = 1, string filterBy = "A-Z");
        Task<List<ManageUserModel>> UserSearchResults(string searchTerm, string roleFilter, int pageNumber = 1);
        Task LockoutUser(User user);
        Task UnlockUser(User user);
        Task<bool> PhoneNumberIsInUse(Guid id, string phoneNumber);
        Task<bool> PhoneNumberIsInUse(string phoneNumber);

    }
}

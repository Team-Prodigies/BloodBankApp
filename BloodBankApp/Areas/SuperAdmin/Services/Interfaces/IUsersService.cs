using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IUsersService
    {
        Task<User> GetUser(Guid id);
        Task<IdentityResult> AddSuperAdmin(SuperAdminModel user);
    }
}

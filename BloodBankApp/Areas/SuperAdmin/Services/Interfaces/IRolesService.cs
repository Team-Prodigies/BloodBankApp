using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IRolesService
    {
        Task<List<IdentityRole<Guid>>> GetAllRoles();
        Task<IdentityResult> CreateRole(RoleModel model);
        Task<IdentityRole<Guid>> GetRole(Guid id);
        Task<IdentityResult> UpdateRole(IdentityRole<Guid> role);
    }
}

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
        Task<List<SelectedRoleModel>> GetAllSelectedRoles();
        Task<IdentityResult> CreateRole(PermissionViewModel model);
        Task<IdentityRole<Guid>> GetRole(Guid id);
        Task<PermissionViewModel> GetRolePermissions(Guid? id);
        Task UpdatePermissions(PermissionViewModel model);
        Task<IdentityResult> UpdateRole(IdentityRole<Guid> role);
    }
}

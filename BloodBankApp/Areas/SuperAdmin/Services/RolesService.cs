using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RolesService(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRole(RoleModel model)
        {
            var role = new IdentityRole<Guid>()
            {
                Name = model.RoleName
            };
            return await _roleManager.CreateAsync(role);
        }

        public async Task<List<IdentityRole<Guid>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole<Guid>> GetRole(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> UpdateRole(IdentityRole<Guid> role)
        {
            return await _roleManager.UpdateAsync(role);
        }
    }
}

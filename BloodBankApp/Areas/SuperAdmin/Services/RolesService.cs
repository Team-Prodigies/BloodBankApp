using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class RolesService : IRolesService
    {
        public Task<IdentityResult> CreateRole(RoleModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole<Guid>>> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole<Guid>> GetRole(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

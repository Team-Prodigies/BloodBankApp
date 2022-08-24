using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersService(UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> AddSuperAdmin(SuperAdminModel user)
        {
            var superAdminAccount = _mapper.Map<User>(user);

            var result = await _userManager.CreateAsync(superAdminAccount, user.Password);

            if (result.Succeeded)
            {
                return await _userManager.AddToRoleAsync(superAdminAccount, "SuperAdmin");
            }
            return IdentityResult.Failed();
        }
    }
}

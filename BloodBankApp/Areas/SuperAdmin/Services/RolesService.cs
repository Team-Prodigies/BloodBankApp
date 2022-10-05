using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BloodBankApp.Areas.Services.Interfaces;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _usersManager;
        private readonly IUsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RolesService(
            RoleManager<IdentityRole<Guid>> roleManager,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<User> signInManager,
            IUsersService usersService,
            UserManager<User> usersManager,
            IMapper mapper)
        {
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _usersService = usersService;
            _mapper = mapper;
            _usersManager = usersManager;
        }

        public async Task<List<string>> GetAllRoleNames()
        {
            var roles = await GetAllRoles();
            var roleNames = roles
                .Where(x=>x.Name!="SuperAdmin")
                .Select(x => x.Name)
                .ToList();

            return roleNames;
        }

        public async Task<List<SelectedRoleModel>> GetAllSelectedRoles()
        {
            var roles = await GetAllRoles();
            var selectedRoles = new List<SelectedRoleModel>();
            roles.ForEach(role => selectedRoles.Add(_mapper.Map<SelectedRoleModel>(role)));

            return selectedRoles;
        }

        public async Task<IdentityResult> CreateRole(PermissionViewModel model)
        {
            var role = new IdentityRole<Guid>()
            {
                Name = model.RoleName
            };
            var result = await _roleManager.CreateAsync(role);
            foreach (var claim in model.RoleClaims)
            {
                if (claim.Selected)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                }
            }
            return result;
        }

        public async Task<List<IdentityRole<Guid>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityRole<Guid>> GetRole(Guid id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<PermissionViewModel> GetRolePermissions(Guid? id)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Cities));
            allPermissions.GetPermissions(typeof(Permissions.Donors));
            allPermissions.GetPermissions(typeof(Permissions.Hospitals));
            allPermissions.GetPermissions(typeof(Permissions.SuperAdmin));
            allPermissions.GetPermissions(typeof(Permissions.Roles));
            allPermissions.GetPermissions(typeof(Permissions.HospitalAdmin));
            if (id is null)
            {
                model.RoleClaims = allPermissions;
                return model;
            }
            var role = await GetRole((Guid)id);
            model.RoleId = (Guid)id;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            model.RoleName = role.Name;
            return model;
        }

        public async Task UpdatePermissions(PermissionViewModel model)
        {
            var role = await GetRole(model.RoleId);
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            foreach (var claim in model.RoleClaims)
            {
                if (claim.Selected)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claim.Type, claim.Value));
                }
            }
            try
            {
                await _signInManager.RefreshSignInAsync(await _usersService.GetUser(_httpContextAccessor.HttpContext.User));
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task<IdentityResult> UpdateRole(IdentityRole<Guid> role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<UserRoleModel> GetUserRoles(Guid id)
        {
            var user = await _usersService.GetUser(id);
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new UserRoleModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Roles = new List<SelectedRoleModel>()
            };

            foreach (var role in roles)
            {
                if (role.Name == "SuperAdmin")
                {
                    continue;
                }
                if (await _usersService.UserIsInRole(user, role.Name))
                {
                    model.Roles.Add(new SelectedRoleModel { RoleName = role.Name, IsSelected = true });
                }
                else
                {
                    model.Roles.Add(new SelectedRoleModel { RoleName = role.Name });
                }
            }
            return model;
        }

        public async Task<IdentityResult> SetUserRoles(UserRoleModel model)
        {
            var user = await _usersService.GetUser(model.UserId);
            var roles = await _usersManager.GetRolesAsync(user);
            var result = await _usersManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (result.Succeeded)
            {
                var selectedRoles = model.Roles.Where(x => x.IsSelected).Select(x => x.RoleName);
                result = await _usersManager.AddToRolesAsync(user, selectedRoles);
            }
            return result;
        }
    }
}
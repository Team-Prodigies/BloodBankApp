using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;
        public RolesController(IRolesService rolesService,
            IMapper mapper,
            INotyfService notyfService)
        {
            _rolesService = rolesService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        [Authorize(Policy = Permissions.Roles.View)]
        public async Task<IActionResult> AllRoles()
        {
            var roles = await _rolesService.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Roles.Create)]
        public async Task<IActionResult> CreateRole()
        {
            var model = await _rolesService.GetRolePermissions(null);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Create)]
        public async Task<IActionResult> CreateRole(PermissionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var check = await _rolesService.CreateRole(model);
            if (!check.Succeeded)
            {
                foreach (var error in check.Errors)
                {
                    if (!ModelState.ContainsKey(error.Code))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                _notyfService.Error("Failed to create role");
                return View(model);
            }
            _notyfService.Success("Role added successfully!");
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Roles.Edit)]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _rolesService.GetRole(id);

            if (role == null)
            {
                _notyfService.Warning("Role was not found!");
                return View();
            }
            var roleModel = _mapper.Map<RoleModel>(role);
            return View(roleModel);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Edit)]
        public async Task<IActionResult> EditRole(RoleModel role, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            if (role.Id != id)
            {
                _notyfService.Warning("Role was not found");
                return View(role);
            }

            var dbRole = await _rolesService.GetRole(id);

            if (dbRole == null)
            {
                _notyfService.Warning("Role was not found");
                return View(role);
            }
            dbRole.Name = role.RoleName;

            var result = await _rolesService.UpdateRole(dbRole);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (!ModelState.ContainsKey(error.Code))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                _notyfService.Error("Role could not be updated");
                return View(role);
            }
            _notyfService.Success("Role was updated!");
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var model = await _rolesService.GetUserRoles(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetUserRoles(UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(GetUserRoles), model);
            }
            var result = await _rolesService.SetUserRoles(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (!ModelState.ContainsKey(error.Code))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                _notyfService.Error("Something went wrong");
                return RedirectToAction(nameof(GetUserRoles), model);
            }
            _notyfService.Success("Roles updated successfully");
            return View(nameof(GetUserRoles), model);
        }
    }
}
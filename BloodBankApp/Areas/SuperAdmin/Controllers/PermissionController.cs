using System;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Authorize]
    [Area("SuperAdmin")]
    public class PermissionController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notyfService;

        public PermissionController(IRolesService rolesService,
            INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }
        [Authorize(Policy = Permissions.Roles.ViewPermissions)]
        public async Task<ActionResult> Index(Guid roleId)
        {
            var model = await _rolesService.GetRolePermissions(roleId);
            return View(model);
        }
        [Authorize(Policy = Permissions.Roles.EditPermissions)]
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            await _rolesService.UpdatePermissions(model);
            _notyfService.Success($"Permissions for role {model.RoleName} updated successfully");
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
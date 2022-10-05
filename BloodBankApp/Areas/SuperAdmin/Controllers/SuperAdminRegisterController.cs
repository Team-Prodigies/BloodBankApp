using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class SuperAdminRegisterController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly INotyfService _notyfService;
        private readonly IRolesService _rolesService;
        public SuperAdminRegisterController(
            IUsersService usersService,
            INotyfService notyfService,
            IRolesService rolesService)
        {
            _usersService = usersService;
            _notyfService = notyfService;
            _rolesService = rolesService;
        }

        [Authorize(Policy = Permissions.SuperAdmin.Create)]
        public async Task<IActionResult> CreateSuperAdmin()
        {
            var roles = await _rolesService.GetAllSelectedRoles();

            var model = new SuperAdminModel
            {
                Roles = roles
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.ViewStatistics)]
        public async Task<IActionResult> CreateSuperAdmin(SuperAdminModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersService.AddSuperAdmin(user);

                if (result.Succeeded)
                {
                    _notyfService.Success("User account was added!");
                    return RedirectToAction(nameof(CreateSuperAdmin));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                _notyfService.Error("Failed to add user!");
            }
            return View(user);
        }
    }
}
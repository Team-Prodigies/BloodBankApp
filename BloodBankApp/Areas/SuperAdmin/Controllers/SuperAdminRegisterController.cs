using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminRegisterController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly INotyfService _notyfService;
        public SuperAdminRegisterController(IUsersService usersService,
            ISignInService signInService, INotyfService notyfService)
        {
            _usersService = usersService;
            _signInService = signInService;
            _notyfService = notyfService;
        }

        public IActionResult CreateSuperAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuperAdmin(SuperAdminModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _usersService.AddSuperAdmin(user);

                if (result.Succeeded)
                {
                    _notyfService.Success("SuperAdmin account was added!");
                    return RedirectToAction(nameof(CreateSuperAdmin));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
    }
}

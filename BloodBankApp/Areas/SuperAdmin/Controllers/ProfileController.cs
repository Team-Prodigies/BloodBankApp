using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        public ProfileController(IUsersService usersService,
            ISignInService signInService,
            INotyfService notyfService, IMapper mapper)
        {
            _usersService = usersService;
            _signInService = signInService;
            _notyfService = notyfService;
            _mapper = mapper;
        }

        [Authorize(Policy = Permissions.SuperAdmin.ViewProfile)]
        public async Task<IActionResult> Index()
        {
            var superAdmin = await _usersService.GetUser(User);
            ProfileAdminModel profileAdminUser = _mapper.Map<ProfileAdminModel>(superAdmin);
            return View(profileAdminUser);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.EditProfile)]
        public async Task<IActionResult> EditProfile(ProfileAdminModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index));
            }

            var result = await _usersService.EditSuperAdmin(user);

            if (result.Succeeded)
            {
                _notyfService.Success("Profile edited successfully!");
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _notyfService.Error(error.Description.Replace("'", ""));
            }
            return View(nameof(Index));
        }

        [Authorize(Policy = Permissions.SuperAdmin.ChangePassword)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.SuperAdmin.ChangePassword)]
        public async Task<IActionResult> ChangeSuperAdminPassword(ChangePasswordModel pass)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ChangePassword));
            }
            var superAdmin = await _usersService.GetUser(User);
            var result = await _usersService.ChangePassword(superAdmin, pass.OldPassword, pass.NewPassword);
            if (result.Succeeded)
            {
                _notyfService.Success("Password changed successfully!");
                await _signInService.RefreshSignInAsync(superAdmin);
                return RedirectToAction(nameof(ChangePassword));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _notyfService.Error(error.Description);
            }
            return View(nameof(ChangePassword));
        }
    }
}
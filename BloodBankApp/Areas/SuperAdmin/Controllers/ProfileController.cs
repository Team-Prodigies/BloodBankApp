using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ProfileController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        public ProfileController(IUsersService usersService, ISignInService signInService, INotyfService notyfService,IMapper mapper)
        {
            _usersService = usersService;
            _signInService = signInService;
            _notyfService = notyfService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var superadmin = await _usersService.GetUser(User);

            ProfileAdminModel profileAdminUser = _mapper.Map<ProfileAdminModel>(superadmin);

            return View(profileAdminUser);
        }

        [HttpPost]
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
                _notyfService.Error(error.Description.Replace("'",""));
            }
            return View(nameof(Index));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSuperAdminPassword(ChangePasswordModel pass)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(ChangePassword));
            }

            var superadmin = await _usersService.GetUser(User);
            var result = await _usersService.ChangePassword(superadmin, pass.OldPassword, pass.NewPassword);


            if (result.Succeeded)
            {
                _notyfService.Success("Password changed successfully!");
                await _signInService.RefreshSignInAsync(superadmin);
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

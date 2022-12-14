using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using BloodBankApp.Models;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly UserManager<User> _userManager;
        private readonly IHospitalAdminService _hospitalAdminService;

        public HomeController(ApplicationDbContext context,
            INotyfService notyfService,
            UserManager<User> userManager,
            IHospitalAdminService hospitalAdminService)
        {
            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
            _hospitalAdminService = hospitalAdminService;
        }

        [Authorize(Policy = Permissions.HospitalAdmin.ViewDashboard)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.ViewProfile)]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var hospitalAdmin = new HospitalAdminModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
            };
            return View(hospitalAdmin);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.EditProfile)]
        public async Task<IActionResult> ChangeProfile(HospitalAdminModel hospitalModel)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Your changes are not correct.");
                return View(nameof(ChangePassword));
            }

            var getUser = await _userManager.GetUserAsync(User);

            if (getUser.Id != hospitalModel.Id)
            {
                _notyfService.Error("Your changes are not right.");
                return View(nameof(Profile));
            }

            var result = await _hospitalAdminService.EditHospitalAdmin(hospitalModel);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyfService.Error(error.Description);
                }
            }

            await _context.SaveChangesAsync();
            _notyfService.Success("You profile changes, changed successfully.");
            return View(nameof(Profile));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.ChangePassword)]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.ChangePassword)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Your password has not changed.");
                return View(nameof(ChangePassword));
            }

            var user = await _userManager.GetUserAsync(User);

            if (changePasswordModel.OldPassword == changePasswordModel.NewPassword)
            {
                _notyfService.Error("Your password cant be same as old password");
                return View(nameof(ChangePassword));
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyfService.Error(error.Description);
                }

                return View(nameof(ChangePassword));
            }

            _notyfService.Success("Your password has been changed.");
            return View(nameof(ChangePassword));
        }
    }
}
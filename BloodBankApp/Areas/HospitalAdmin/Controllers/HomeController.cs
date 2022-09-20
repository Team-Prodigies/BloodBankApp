using AutoMapper;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using BloodBankApp.ExtensionMethods;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{       
    [Area("HospitalAdmin")]
    [Authorize(Roles="HospitalAdmin")]
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyfService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHospitalAdminService _hospitalAdminService;

        public HomeController(ApplicationDbContext context,
            INotyfService notyfService,
            UserManager<User> userManager,
            IMapper mapper,
            IHospitalAdminService hospitalAdminService) {

            _context = context;
            _notyfService = notyfService;
            _userManager = userManager;
            _mapper = mapper;
            _hospitalAdminService = hospitalAdminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profile() {
            var user = _userManager.GetUserAsync(User);

            var hospitalAdmin = new HospitalAdminModel {
                Id = user.Result.Id,
                Name = user.Result.Name,
                Surname = user.Result.Surname,
                UserName = user.Result.UserName,
                PhoneNumber = user.Result.PhoneNumber,
                DateOfBirth = user.Result.DateOfBirth,
            };
            return View(hospitalAdmin);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfile(HospitalAdminModel hospitalModel) {
            if (!ModelState.IsValid) {
                _notyfService.Error("You changes are not correct.");
                return View(nameof(ChangePassword));
            }
            var getUser = _userManager.GetUserAsync(User);
            
            if(getUser.Result.Id != hospitalModel.Id) {
                _notyfService.Error("Your changes are not right.");
                return View(nameof(Profile));
            }
            var result = await _hospitalAdminService.EditHospitalAdmin(hospitalModel);
            
            if (!result.Succeeded) {
                foreach (var error in result.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyfService.Error(error.Description);
                }
            }
                _context.SaveChanges();
                _notyfService.Success("You profile changes, changed successfuly.");
                return View(nameof(Profile));
        }

       [HttpGet]
       public IActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel) {
            if (!ModelState.IsValid) {
                _notyfService.Error("Your password has not changed.");
                return View(nameof(ChangePassword));
            }
            var user = await _userManager.GetUserAsync(User);

            if (changePasswordModel.OldPassword == changePasswordModel.NewPassword) {
                _notyfService.Error("Your password cant be same as old password");
                return View(nameof(ChangePassword));
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword, changePasswordModel.NewPassword);
            if (!changePasswordResult.Succeeded) {
                foreach (var error in changePasswordResult.Errors) {
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

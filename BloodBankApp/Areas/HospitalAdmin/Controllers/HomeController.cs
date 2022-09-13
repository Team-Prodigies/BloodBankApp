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
            var getUser = _userManager.GetUserAsync(User);
            
            if(getUser.Result.Id != hospitalModel.Id) {
                _notyfService.Error("Your changes are not right.");
                return RedirectToAction("Profile");
            }
            var result = await _hospitalAdminService.EditHospitalAdmin(hospitalModel);
            var changeInfo = await _userManager.UpdateAsync(getUser.Result);
            
            if (!changeInfo.Succeeded) {
                foreach (var error in changeInfo.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyfService.Error(error.Description);
                }
            }
                _context.SaveChanges();
                _notyfService.Success("You profile changes, changed successfuly.");
                return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> ChagePassword(HospitalAdminModel hospitalAdmin) {
            if (!ModelState.IsValid) {
                _notyfService.Error("Your password has not changed.");
                return RedirectToAction("Profile");
            }
            var user = await _userManager.GetUserAsync(User);

            if (hospitalAdmin.OldPassword == hospitalAdmin.NewPassword) {
                _notyfService.Error("Your password cant be same as old password");
                return RedirectToAction("Profile");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, hospitalAdmin.OldPassword, hospitalAdmin.NewPassword);
            if (!changePasswordResult.Succeeded) {
                foreach (var error in changePasswordResult.Errors) {
                    ModelState.AddModelError(string.Empty, error.Description);
                    _notyfService.Error(error.Description);
                }
                return RedirectToAction("Profile");
            }

            _notyfService.Success("Your password has been changed.");
            return RedirectToAction("Profile");
        }
    }
}

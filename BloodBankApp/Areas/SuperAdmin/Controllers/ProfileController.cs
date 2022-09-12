using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.ExtensionMethods;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var admin = new SuperAdminModel();
            var superadmin = await _usersService.GetUser(User);
            admin.UserName = superadmin.UserName;
            admin.Name = superadmin.Name;
            admin.Surname = superadmin.Surname;
            admin.DateOfBirth = superadmin.DateOfBirth;
            admin.Email = superadmin.Email;
            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SuperAdminModel user)
        {
            user.Name = user.Name.ToTitleCase();
            user.Surname = user.Surname.ToTitleCase();
            var superadmin = await _usersService.GetUser(User);
            superadmin.Name = user.Name;
            superadmin.Surname = user.Surname;
            superadmin.UserName = user.UserName;
            superadmin.DateOfBirth = user.DateOfBirth;
            superadmin.Email = user.Email;
            var result = await _usersService.EditSuperAdmin(superadmin);

            if (result.Succeeded)
            {
                _notyfService.Success("Profile edited successfully!");
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _notyfService.Error("Something went wrong!");
            }
            return View(nameof(Index));
        }

        public IActionResult ChangePassword()
        {
            return View();
        }






    }
}

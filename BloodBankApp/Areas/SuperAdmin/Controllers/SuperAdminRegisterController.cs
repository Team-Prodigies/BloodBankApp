using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterModel;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminRegisterController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        public List<AuthenticationScheme> ExternalLogins { get; private set; }

        public SuperAdminRegisterController(IUsersService usersService,
            ISignInService signInService)
        {
            _usersService = usersService;
            _signInService = signInService;
        }

        public IActionResult CreateSuperAdmin()
        {
            return View();
        }
        public IActionResult AccountCreatedSuccessfully()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuperAdmin(SuperAdminModel user)
        {
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var result = await _usersService.AddSuperAdmin(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(AccountCreatedSuccessfully));
                }
            }
            return RedirectToAction(nameof(CreateSuperAdmin));
        }
    }
}

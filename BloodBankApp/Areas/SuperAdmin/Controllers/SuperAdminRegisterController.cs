using AutoMapper;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public List<AuthenticationScheme> ExternalLogins { get; private set; }

        public SuperAdminRegisterController(SignInManager<User> signInManager,
           UserManager<User> userManager, IMapper mapper)
        {

            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;

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

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                User superAdminAccount = _mapper.Map<User>(user);

                var result = await _userManager.CreateAsync(superAdminAccount, user.Password);

                await _userManager.AddToRoleAsync(superAdminAccount, "SuperAdmin");

                if (result.Succeeded)
                {
                    return RedirectToAction("AccountCreatedSuccessfully");
                }

            }

            return RedirectToAction("CreateSuperAdmin");

        }

    }
}

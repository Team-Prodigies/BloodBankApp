using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static BloodBankApp.Areas.Identity.Pages.Account.RegisterModel;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    public class SuperAdminRegisterController : Controller {

        private readonly ApplicationDbContext context;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        public List<AuthenticationScheme> ExternalLogins { get; private set; }

        public SuperAdminRegisterController(ApplicationDbContext context, SignInManager<User> signInManager,
           UserManager<User> userManager, IMapper mapper) {

            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;

        }

        public IActionResult CreateSuperAdmin() {
            return View();
        }
        public IActionResult AccountCreatedSuccessfully() {
            return View();
        }

        [HttpPost]
            public async Task<IActionResult> CreateSuperAdmin(SuperAdminModel user) {
          
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid) {

                 User superAdminAccount = mapper.Map<User>(user);
                  
                 var result = await userManager.CreateAsync(superAdminAccount, user.Password);

                 await userManager.AddToRoleAsync(superAdminAccount, "SuperAdmin");

                if (result.Succeeded) {
                    return RedirectToAction("AccountCreatedSuccessfully");
                }

            }

             return RedirectToAction("CreateSuperAdmin");

        }

    }
}

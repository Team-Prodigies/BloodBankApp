﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BloodBankApp.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.CustomValidation;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly IBloodTypesService _bloodTypesService;
        private readonly ICitiesService _citiesService;

        public RegisterModel(
            IUsersService usersService,
            ISignInService signInService,
            IBloodTypesService bloodTypesService,
            ICitiesService citiesService)
        {
            _usersService = usersService;
            _signInService = signInService;
            _bloodTypesService = bloodTypesService;
            _citiesService = citiesService;
            CityList = new SelectList(_citiesService.GetCities().Result, "CityId", "CityName");
            BloodTypeList = new SelectList(_bloodTypesService.GetAllBloodTypes().Result, "BloodTypeId", "BloodTypeName");
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        private SelectList CityList { get; set; }
        private SelectList BloodTypeList { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class RegisterInputModel
        {
            [Required]
            [Numbers]
            public string Name { get; set; }

            [Numbers]
            [Required]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "Username")]
            [StringLength(30,ErrorMessage ="Username cannot be longer than 20 characters")]
            public string UserName { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
            [Display(Name = "Date of Birthday")]
            [DataType(DataType.Date)]
            [MinAgeAttribute(18)]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // Donor Fields
            [Required]
            [PersonalNumberAttribute]
            public long PersonalNumber { get; set; }

            [Required]
            public Gender Gender { get; set; }

            [Required]
            [Display(Name = "Blood Type")]
            public Guid BloodTypeId { get; set; }

            [Required]
            [Display(Name = "City")]
            public Guid CityId { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["City"] = CityList;
            ViewData["BloodType"] = BloodTypeList;

            ReturnUrl = returnUrl;

            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _usersService.AddDonor(Input);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["City"] = CityList;
            ViewData["BloodType"] = BloodTypeList;

            return Page();
        }
    }
}

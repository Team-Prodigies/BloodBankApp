using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.CustomValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterMedicalStaffModel : PageModel
    {
        private readonly ISignInService _signInService;
        private readonly IUsersService _usersService;
        private readonly IHospitalService _hospitalService;

        public RegisterMedicalStaffModel(
            ISignInService signInService,
            IUsersService usersService,
            IHospitalService hospitalService)
        {
            _signInService = signInService;
            _usersService = usersService;
            _hospitalService = hospitalService;

            HospitalList = new SelectList(_hospitalService.GetAllHospitals().Result, "HospitalId", "HospitalName");
        }

        [BindProperty]
        public RegisterMedicalStaffInputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        private SelectList HospitalList { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterMedicalStaffInputModel
        {
            [Required]
            [Numbers]
            public string Name { get; set; }

            [Numbers]
            [Required]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
            [Display(Name = "Date of birth")]
            [DataType(DataType.Date)]
            [MinAge(18)]
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

            // MedicalStaff Field

            [Required]
            [Display(Name = "Hospital")]
            public Guid HospitalId { get; set; }

            [Display(Name = "Hospital code")]
            public string HospitalCode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Hospital"] = HospitalList;

            ReturnUrl = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _usersService.AddHospitalAdmin(Input);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "HospitalAdmin"});
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["Hospital"] = HospitalList;
            return Page();
        }
    }
}

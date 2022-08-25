using AutoMapper;
using BloodBankApp.Areas.Identity.Services;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterMedicalStaffModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly ISignInService _signInService;
        private readonly IMedicalStaffService _medicalStaffService;
        private readonly IUsersService _usersService;
        private readonly ApplicationDbContext _context;

        public RegisterMedicalStaffModel(
            IEmailSender emailSender, 
            IMapper mapper,
            ISignInService signInService,
            IMedicalStaffService medicalStaffService,
            IUsersService usersService,
            ApplicationDbContext context)
        {
            _mapper = mapper;
            _signInService = signInService;
            _medicalStaffService = medicalStaffService;
            _usersService = usersService;
            _context = context;

            HospitalList = new SelectList(_context.Hospitals.ToList(), "HospitalId", "HospitalName");
        }

        [BindProperty]
        public RegisterMedicalStaffInputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        private SelectList HospitalList { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterMedicalStaffInputModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
            [Display(Name = "Date of Birthday")]
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

            public String HospitalCode { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["Hospital"] = HospitalList;

            ReturnUrl = returnUrl;

            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInService.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var result = await _usersService.AddHospitalAdmin(Input);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If we got this far, something failed, redisplay form
            ViewData["Hospital"] = HospitalList;
            return Page();
        }
    }
}

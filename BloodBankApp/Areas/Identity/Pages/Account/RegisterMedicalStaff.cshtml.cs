using AutoMapper;
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
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RegisterMedicalStaffModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, IMapper mapper,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
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

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var hospitalCode = Input.HospitalCode;

                    var hospital = await _context.Hospitals
                               .Where(x => x.HospitalId == Input.HospitalId).FirstOrDefaultAsync();

                    if (!hospital.HospitalCode.Equals(Input.HospitalCode))
                    {
                        return BadRequest("This code doesnt belong to the hospital you entered");
                    }
                    var user = _mapper.Map<User>(Input);

                    user.Id = Guid.NewGuid();

                    var medicalStaff = _mapper.Map<MedicalStaff>(Input);

                    try
                    {
                        var result = await _userManager.CreateAsync(user, Input.Password);

                        await _userManager.AddToRoleAsync(user, "MedicalStaff");

                        if (result.Succeeded)
                        {
                            medicalStaff.MedicalStaffId = user.Id;

                            await _context.MedicalStaffs.AddAsync(medicalStaff);

                            await _context.SaveChangesAsync();

                            transaction.Commit();

                            _logger.LogInformation("User created a new account with password.");

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            await _signInManager.SignInAsync(user, isPersistent: false);

                            return LocalRedirect(returnUrl);
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            ViewData["Hospital"] = HospitalList;
            return Page();
        }
    }
}

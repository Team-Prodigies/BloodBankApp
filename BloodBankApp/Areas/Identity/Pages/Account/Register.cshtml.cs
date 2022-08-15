using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using BloodBankApp.Enums;
using AutoMapper;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _mapper = mapper;
            _context = context;

            CityList = new SelectList(_context.Cities.ToList(), "CityId", "CityName");
            BloodTypeList = new SelectList(_context.BloodTypes.ToList(), "BloodTypeId", "BloodTypeName");
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public string ReturnUrl { get; set;  }
        private SelectList CityList { get; set;  } 
        private SelectList BloodTypeList { get; set;  }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class RegisterInputModel
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
                    var user = _mapper.Map<User>(Input);

                    user.Id = Guid.NewGuid();

                    var donor = _mapper.Map<Donor>(Input);

                    try
                    {
                        var result = await _userManager.CreateAsync(user, Input.Password);

                        await _userManager.AddToRoleAsync(user, "Donor");

                        if (result.Succeeded)
                        {
                            donor.DonorId = user.Id;

                            await _context.Donors.AddAsync(donor);

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

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                return LocalRedirect(returnUrl);
                            }
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
            // If we got this far, something failed, redisplay form

            ViewData["City"] = CityList;
            ViewData["BloodType"] = BloodTypeList;
            
            return Page();
        }
    }
}

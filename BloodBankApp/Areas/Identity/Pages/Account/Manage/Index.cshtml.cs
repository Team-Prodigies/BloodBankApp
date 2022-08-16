using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BloodBankApp.Areas.Identity.Pages.Account.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.Identity.Pages.Account.Manage
{
    public partial class PersonalProfileIndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PersonalProfileIndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }

        public string Username { get; set; }

        public long PersonalNumber { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "BloodType")]
        public string BloodTypeName{ get; set; }

        public String Surname { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


        }

        private async Task LoadAsync(User user)
        {
            var donor = await _context.Donors
                .Select(d => new Donor
                {
                    DonorId = d.DonorId,
                    User = d.User,
                    PersonalNumber = d.PersonalNumber,
                    Gender = d.Gender,
                    BloodType = d.BloodType,
                    City = d.City
                }).FirstOrDefaultAsync(x => x.DonorId == user.Id);
           
            var donorDto = _mapper.Map<DonorDto>(donor);

            var userName = user.UserName;

            var personalNumber = donor.PersonalNumber;

            var gender = donor.Gender;

            var bloodTypeName = donorDto.BloodTypeName;

            var surname = user.Surname;

            var dOB = user.DateOfBirth;

            var phoneNumber = user.PhoneNumber;

             var cityName = donorDto.CityName;

            Username = userName;
            PersonalNumber = personalNumber;
            Gender = gender;
            BloodTypeName = bloodTypeName;
            Surname = surname;
            DateOfBirth = dOB;
            CityName = cityName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}

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
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Display(Name = "Personal number")]
        public long PersonalNumber { get; set; }

        public Gender Gender { get; set; }

        [Display(Name = "Blood Type")]
        public string BloodTypeName{ get; set; }

        public string Name { get; set; }

        public String Surname { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

      

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Display(Name = "City")]
            public Guid CityId { get; set; }


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

            var name = user.Name;

            var surname = user.Surname;

            var dOB = user.DateOfBirth;

            var phoneNumber = user.PhoneNumber;

             var cityId = donor.CityId;

           
            PersonalNumber = personalNumber;
            Gender = gender;
            BloodTypeName = bloodTypeName;
            Name = name;
            Surname = surname;
            DateOfBirth = dOB;
           

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                UserName = userName,
                CityId = cityId
        };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            ViewData["City"] = new SelectList(_context.Cities.ToList(), "CityId", "CityName");
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(User userModel)
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
            var donor = await _context.Donors.FindAsync(user.Id);
            var phoneNumber = user.PhoneNumber;
            var userName = user.UserName;
            var cityId = donor.CityId;

            var result = _userManager.UpdateAsync(userModel);

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (Input.UserName != userName)
            {
                user.UserName = Input.UserName;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            if (Input.CityId != cityId)
            {
                donor.CityId = Input.CityId;
                _context.Update(donor);
                await _context.SaveChangesAsync();
            }


            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            ViewData["City"] = new SelectList(_context.Cities.ToList(), "CityId", "CityName");
            return RedirectToPage();
        }
    }
}

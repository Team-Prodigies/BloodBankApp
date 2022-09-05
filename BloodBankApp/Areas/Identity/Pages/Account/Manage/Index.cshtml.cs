using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBankApp.Areas.Identity.Pages.Account.Manage
{ 
    [Authorize(Roles = "Donor")]
    public class PersonalProfileIndexModel : PageModel
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly ICitiesService _citiesService;
        private readonly IDonorsService _donorsService;
        private readonly INotyfService _notyfService;
        public PersonalProfileIndexModel(
            IUsersService usersService,
            ISignInService signInService,
            ICitiesService citiesService,
            IDonorsService donorsService,
            INotyfService notyfService)
        {
            _usersService = usersService;
            _signInService = signInService;
            _citiesService = citiesService;
            _donorsService = donorsService;
            _notyfService = notyfService;
            CityList = new SelectList(_citiesService.GetCities().Result, "CityId", "CityName");
        }

        [Display(Name = "Personal number")]
        public long PersonalNumber { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Blood Type")]
        public string BloodTypeName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }
        private SelectList CityList { get; set; }

        public class ProfileInputModel
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
            var donor = await _donorsService.GetDonor(user.Id);

            PersonalNumber = donor.PersonalNumber;
            Gender = donor.Gender;
            BloodTypeName = donor.BloodType.BloodTypeName;
            Name = user.Name;
            Surname = user.Surname;
            DateOfBirth = user.DateOfBirth;

            Input = new ProfileInputModel
            {
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                CityId = donor.CityId
            };
        }
       
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _usersService.GetUser(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_usersService.GetUserId(User)}'.");
            }
            ViewData["City"] = CityList;
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _usersService.GetUser(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_usersService.GetUserId(User)}'.");
            }
            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                ViewData["City"] = CityList;
                return Page();
            }
            var donor = await _donorsService.GetDonor(user.Id);
            var phoneNumber = user.PhoneNumber;
            var userName = user.UserName;
            var cityId = donor.CityId;

            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _usersService.SetPhoneNumber(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    _notyfService.Error("Unexpected error when trying to set phone number.");
                    return RedirectToPage();
                }
            }
            if (Input.UserName != userName)
            {
                var setUserNameResult = await _usersService.SetUserName(user, Input.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    _notyfService.Error("Unexpected error when trying to set username.");
                    return RedirectToPage();
                }
            }
            if (Input.CityId != cityId)
            {
                donor.CityId = Input.CityId;
                await _donorsService.EditDonor(donor);
            }

            await _signInService.RefreshSignInAsync(user);
            _notyfService.Success("Your profile has been updated");
            ViewData["City"] = CityList;
            return RedirectToPage();
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBankApp.Areas.Identity.Pages.Account.Manage
{ 
    [Authorize(Policy = Permissions.Donors.ViewProfile)]
    [Authorize(Policy = Permissions.Donors.EditProfile)]
    public class PersonalProfileIndexModel : PageModel
    {
        private readonly IUsersService _usersService;
        private readonly ISignInService _signInService;
        private readonly ICitiesService _citiesService;
        private readonly IDonorsService _donorsService;
        private readonly INotyfService _notyfService;
        private readonly IBloodTypesService _bloodTypeService;


        public PersonalProfileIndexModel(
            IUsersService usersService,
            ISignInService signInService,
            ICitiesService citiesService,
            IDonorsService donorsService,
            INotyfService notyfService,
            IBloodTypesService bloodTypeService)
        {
            _usersService = usersService;
            _signInService = signInService;
            _citiesService = citiesService;
            _donorsService = donorsService;
            _notyfService = notyfService;
            _bloodTypeService = bloodTypeService;
            CityList = new SelectList(_citiesService.GetCities().Result, "CityId", "CityName");
            BloodTypeList = new SelectList(_bloodTypeService.GetAllBloodTypes().Result, "BloodTypeId", "BloodTypeName");
            GenderList = new SelectList(_donorsService.GetGenders(), "Gender");


        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileInputModel Input { get; set; }
        private SelectList CityList { get; set; }
        private SelectList BloodTypeList { get; set; }
        private SelectList GenderList { get; set; }


        public class ProfileInputModel
        {
            public string Name { get; set; }
            public string Surname { get; set; }

            [Display(Name = "Date of birth")]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Display(Name = "Personal number")]
            public long PersonalNumber { get; set; }
            public Gender Gender { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Display(Name = "Blood Type")]
            public Guid? BloodTypeId { get; set; }

            [Display(Name = "City")]
            public Guid CityId { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var donor = await _donorsService.GetDonor(user.Id);

            Input = new ProfileInputModel
            {
                PersonalNumber = donor.PersonalNumber,
                Gender = donor.Gender,
                Name = user.Name,
                Surname = user.Surname,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                CityId = donor.CityId,
                BloodTypeId = donor.BloodTypeId
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
            ViewData["BloodType"] = BloodTypeList;
            ViewData["Gender"] = GenderList;

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
                ViewData["BloodType"] = BloodTypeList;
                ViewData["Gender"] = GenderList;
                return Page();
            }
            var personalNumberTaken = await _donorsService.PersonalNumberIsInUse(user.Id, Input.PersonalNumber);
            var phoneNumberInUse = await _usersService.PhoneNumberIsInUse(user.Id, Input.PhoneNumber);
            if(personalNumberTaken && phoneNumberInUse)
            {
                ViewData["PersonalNumberInUse"] = "This personal number is already taken!";
                ViewData["PhoneNumberInUse"] = "This phone number is already taken!";
                ViewData["City"] = CityList;
                ViewData["BloodType"] = BloodTypeList;
                ViewData["Gender"] = GenderList;
                return Page();
            }
           else if (personalNumberTaken)
            {
                ViewData["PersonalNumberInUse"] = "This personal number is already taken!";
                ViewData["City"] = CityList;
                ViewData["BloodType"] = BloodTypeList;
                ViewData["Gender"] = GenderList;
                return Page();
            }
            else if (phoneNumberInUse)
            {
                ViewData["PhoneNumberInUse"] = "This phone number is already taken!";
                ViewData["City"] = CityList;
                ViewData["BloodType"] = BloodTypeList;
                return Page();
            }
            var result = await _donorsService.EditDonor(user.Id, Input);
            if (!result)
            {
                return RedirectToPage();
            }

            await _signInService.RefreshSignInAsync(user);

            _notyfService.Success("Your profile has been updated");

            ViewData["City"] = CityList;
            ViewData["BloodType"] = BloodTypeList;
            ViewData["Gender"] = GenderList;

            return RedirectToPage();
        }
    }
}
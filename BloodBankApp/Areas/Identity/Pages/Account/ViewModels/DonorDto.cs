﻿using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.Identity.Pages.Account.ViewModels
{
    public class DonorDto
    {
        [Display(Name = "BloodType")]
        public string BloodTypeName { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
    }
}

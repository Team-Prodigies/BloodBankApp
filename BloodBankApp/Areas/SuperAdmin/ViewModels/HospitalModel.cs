using BloodBankApp.CustomValidation;
using BloodBankApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class HospitalModel
    {
        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(200)]
        [Numbers]
        public string HospitalName { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "Hospital code")]
        [StringLength(50,MinimumLength = 2)]
        public string HospitalCode { get; set; }

        [Required]
        [Display(Name = "Location")]
        public Guid LocationId { get; set; }
        public Location Location { get; set; }

        [Required]
        [Display(Name = "City")]
        public Guid CityId { get; set; }
        public City City { get; set; }
    }
}
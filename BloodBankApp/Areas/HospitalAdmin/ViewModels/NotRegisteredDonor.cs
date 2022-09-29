using System;
using System.ComponentModel.DataAnnotations;
using BloodBankApp.CustomValidation;
using BloodBankApp.Enums;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class NotRegisteredDonor
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [MinAge(18)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Personal Number")]
        [PersonalNumber]
        public long PersonalNumber { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Blood Type")]
        public Guid BloodTypeId { get; set; }
        public BloodType BloodType { get; set; }

        [Required]
        [Display(Name = "City")]
        public Guid CityId { get; set; }
        public City City { get; set; }

        [Required]
        [Display(Name = "Code")]
        public Guid CodeId { get; set; }

        public Code Code { get; set; }
      
    }
}

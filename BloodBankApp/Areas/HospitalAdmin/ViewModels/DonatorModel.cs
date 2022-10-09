using System.ComponentModel.DataAnnotations;
using System;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class DonatorModel
    {
        public Guid DonorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Personal number")]
        public long PersonalNumber { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Blood Type")]
        public string BloodTypeName { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
        public bool Locked { get; set; }
    }
}
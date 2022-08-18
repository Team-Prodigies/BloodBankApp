using BloodBankApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class DonorModel
    {
        public virtual bool Locked { get; set; }
        public Guid DonorId { get; set; }
        public String Name { get; set; }

        public String Surname { get; set; }

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
    }
}

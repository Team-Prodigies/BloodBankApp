using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class HospitalAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
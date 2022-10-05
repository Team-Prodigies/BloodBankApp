using BloodBankApp.CustomValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class ProfileAdminModel
    {
        [Required]
        [StringLength(100)]
        [Numbers]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [Numbers]
        public string Surname { get; set; }

        [Display(Name = "Date of birth")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(30, ErrorMessage = "Username cannot be longer than 20 characters")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
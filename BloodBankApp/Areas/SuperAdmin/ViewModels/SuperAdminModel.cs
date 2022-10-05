using BloodBankApp.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class SuperAdminModel
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

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public List<SelectedRoleModel> Roles { get; set; }
    }
}
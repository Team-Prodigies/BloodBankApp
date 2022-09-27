using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        public bool Locked { get; set; }
        public virtual Donor Donor { get; set; }
        public virtual MedicalStaff MedicalStaff { get; set; }      
    }
}

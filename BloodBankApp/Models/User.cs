using Microsoft.AspNetCore.Identity;
using System;

namespace BloodBankApp.Models
{
    public class User : IdentityUser<Guid>
    {
        public String Name { get; set; }

        public String Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual SuperAdmin SuperAdmin { get; set; }

        public virtual Donor Donor { get; set; }

        public virtual MedicalStaff MedicalStaff { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class ManageUserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        public bool Locked { get; set; }
    }
}

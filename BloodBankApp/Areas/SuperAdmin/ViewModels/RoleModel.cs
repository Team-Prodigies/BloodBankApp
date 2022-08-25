using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class RoleModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}

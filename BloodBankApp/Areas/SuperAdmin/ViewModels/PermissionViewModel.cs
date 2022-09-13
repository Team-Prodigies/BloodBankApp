using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BloodBankApp.CustomValidation;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class PermissionViewModel
    {

        public Guid RoleId { get; set; }
        [Required]
        [Display(Name = "Role Name")]
        [Numbers]
        public string RoleName { get; set; }
        public IList<RoleClaimsViewModel> RoleClaims { get; set; }
    }

    public class RoleClaimsViewModel
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}

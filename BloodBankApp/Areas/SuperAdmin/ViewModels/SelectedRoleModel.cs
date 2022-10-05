using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class SelectedRoleModel
    {
        
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
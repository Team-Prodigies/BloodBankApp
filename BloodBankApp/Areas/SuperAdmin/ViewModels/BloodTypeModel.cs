using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class BloodTypeModel
    {
        [Required]
        [Display(Name = "Blood type")]
        public string BloodTypeName { get; set; }
    }
}

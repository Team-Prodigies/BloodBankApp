using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class BloodTypeModel
    {
        public Guid BloodTypeId { get; set; }
        [Required]
        [Display(Name = "Blood type")]
        public string BloodTypeName { get; set; }
    }
}

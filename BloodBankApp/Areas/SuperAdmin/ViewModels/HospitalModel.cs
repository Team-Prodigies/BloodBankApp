using BloodBankApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class HospitalModel
    {

        public Guid HospitalId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(200)]
        public String HospitalName { get; set; }

        [Required]
        [Display(Name = "Contact number")]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public String ContactNumber { get; set; }

        [Required]
        [Display(Name = "Hospital code")]
        [StringLength(50,MinimumLength = 2)]
        public String HospitalCode { get; set; }


        [Required]
        [Display(Name = "Location")]
        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        [Required]
        [Display(Name = "City")]
        public Guid CityId { get; set; }
    }
}

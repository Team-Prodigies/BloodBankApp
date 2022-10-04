using System;
using System.ComponentModel.DataAnnotations;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class EditHospitalModel
    {
        public Guid HospitalId { get; set; }
        [Display(Name = "Hospital Name")]
        public string HospitalName { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        public string? HospitalCode { get; set; }
        [Display(Name= "Location" )]
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        [Display(Name = "City")]
        public Guid CityId { get; set; }
        public City City { get; set; }
    }
}

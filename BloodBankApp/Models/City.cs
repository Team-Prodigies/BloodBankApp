using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class City
    {
        [Key]
        public Guid CityId { get; set; } 

        [Required]
        [StringLength(100)]
        [Display(Name = "City")]
        public String CityName { get; set; }
        public ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();

        public ICollection<Donor> Donors { get; set; } = new List<Donor>();
    }
}

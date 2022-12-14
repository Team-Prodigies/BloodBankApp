using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; }

        [Required]
        [StringLength(50)]
        public string Longitude { get; set; }

        [Required]
        [StringLength(50)]
        public string Latitude { get; set; }
        public ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();
    }
}
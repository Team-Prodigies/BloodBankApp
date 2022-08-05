using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Location
    {
        [Key]
        public Guid LocationId { get; set; }
        public ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();
    }
}

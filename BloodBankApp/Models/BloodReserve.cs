    using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class BloodReserve
    {
        [Key]
        public Guid BloodReserveId { get; set; }
        public double Amount { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public Guid BloodTypeId { get; set; }
        public BloodType BloodType { get; set; }

      
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class MedicalStaff
    {
        [Key]
        public Guid MedicalStaffId { get; set; }
        public virtual User User { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
}
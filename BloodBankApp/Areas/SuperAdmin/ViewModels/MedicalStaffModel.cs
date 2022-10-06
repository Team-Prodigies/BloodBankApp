using BloodBankApp.Models;
using System;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class MedicalStaffModel
    {
        public Guid MedicalStaffId { get; set; }
        public virtual User User { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
}
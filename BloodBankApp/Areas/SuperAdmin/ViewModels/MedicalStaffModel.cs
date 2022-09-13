using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

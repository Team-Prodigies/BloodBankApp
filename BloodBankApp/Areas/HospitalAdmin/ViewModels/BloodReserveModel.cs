using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class BloodReserveModel
    {
        public Guid? BloodReserveId { get; set; }
        public Guid? BloodTypeId { get; set; }
        [Display(Name = "Blood type")]
        public string BloodType { get; set; }
        public double Amount { get; set; }
    }
}

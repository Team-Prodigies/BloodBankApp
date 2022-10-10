using BloodBankApp.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class BloodDonationModel
    {
        public Guid BloodDonationId { get; set; }

        [Required]
        [Display(Name = "Donation date")]
        public DateTime DonationDate { get; set; }

        [Required]
        public double Amount { get; set; }
        public Guid DonationPostId { get; set; }
        public DonationPost DonationPost { get; set; }
        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
    }
}

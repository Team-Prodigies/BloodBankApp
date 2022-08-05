using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class BloodDonation
    {
        [Key]
        public Guid BloodDonationId { get; set; }

        public DateTime DonationDate { get; set; }

        public double Amount { get; set; }

        public Guid DonationPostId { get; set; }

        public DonationPost DonationPost { get; set; }

        public Guid DonorId { get; set; }

        public Donor Donor { get; set; }
    }
}

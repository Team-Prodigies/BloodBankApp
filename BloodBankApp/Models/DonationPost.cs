using BloodBankApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class DonationPost
    {
        [Key]
        public Guid DonationPostId { get; set; }
        public DateTime DateRequired { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Display(Name = "Amount requested")]
        public double AmountRequested { get; set; }

        [Display(Name = "Status")]
        public PostStatus PostStatus { get; set; }
        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public Guid BloodTypeId { get; set; }
        public BloodType BloodType { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();
    }
}
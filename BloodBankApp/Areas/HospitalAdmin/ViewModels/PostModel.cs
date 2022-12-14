using BloodBankApp.Enums;
using BloodBankApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class PostModel
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
    }
}
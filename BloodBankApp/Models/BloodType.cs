using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class BloodType
    {
        [Key]
        public Guid BloodTypeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Blood type")]
        public string BloodTypeName { get; set; }
        public ICollection<BloodReserve> BloodReserves { get; set; } = new List<BloodReserve>();
        public ICollection<DonationPost> DonationPosts { get; set; } = new List<DonationPost>();
        public ICollection<Donor> Donors { get; set; } = new List<Donor>();
    }
}

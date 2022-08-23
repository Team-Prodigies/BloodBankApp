using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BloodBankApp.Models
{
    public class Hospital
    {
        [Key]
        public Guid HospitalId { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(200)]
        public String HospitalName { get; set; }
        [Required]
        [Display(Name = "Contact number")]
        [StringLength(20)]
        public String ContactNumber { get; set; }
        [Required]
        [Display(Name = "Hospital Code")]
        [StringLength(50)]
        public String HospitalCode { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public ICollection<MedicalStaff> MedicalStaff { get; set; } = new List<MedicalStaff>();
        public ICollection<BloodReserve> BloodReserves { get; set; } = new List<BloodReserve>();
        public ICollection<DonationPost> DonationPosts { get; set; } = new List<DonationPost>();
    }
}

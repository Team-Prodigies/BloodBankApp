using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Hospital
    {
        [Key]
        public Guid HospitalId { get; set; }

        public String HospitalName { get; set; }

        public String ContactNumber { get; set; }

        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        public Guid CityId { get; set; }

        public City City { get; set; }
        public ICollection<MedicalStaff> MedicalStaff { get; set; } = new List<MedicalStaff>();

        public ICollection<BloodReserve> BloodReserves { get; set; } = new List<BloodReserve>();

        public ICollection<DonationPost> DonationPosts { get; set; } = new List<DonationPost>();
    }
}

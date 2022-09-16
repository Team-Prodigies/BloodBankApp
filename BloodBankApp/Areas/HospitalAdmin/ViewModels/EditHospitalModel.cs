using System;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class EditHospitalModel
    {
        public Guid HospitalId { get; set; }

        public string HospitalName { get; set; }
        
        public string ContactNumber { get; set; }

        public string HospitalCode { get; set; }

        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
    }
}

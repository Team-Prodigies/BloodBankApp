using System;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class WaitingDonor
    {
        public Guid DonorId { get; set; }
        public Guid HospitalId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
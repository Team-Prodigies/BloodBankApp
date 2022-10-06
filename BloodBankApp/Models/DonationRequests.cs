using System;

namespace BloodBankApp.Models {
    public class DonationRequests 
    {
        public Guid Id { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid DonationPostId { get; set; }
        public DonationPost DonationPost { get; set; }
        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }
    }
}

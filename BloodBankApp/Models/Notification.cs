using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        public String Description { get; set; }

        public Guid DonationPostId { get; set; }

        public DonationPost DonationPost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public Guid? DonationPostId { get; set; }
        public DonationPost DonationPost { get; set; }
        public ICollection<UserNotifications> Users { get; set; } = new List<UserNotifications>();

    }
}
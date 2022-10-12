using System;

namespace BloodBankApp.Models
{
    public class UserNotifications
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}

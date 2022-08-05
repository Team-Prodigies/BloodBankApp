﻿using BloodBankApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class DonationPost
    {
        [Key]
        public Guid NotificationId { get; set; }

        public DateTime DateRequired { get; set; }

        public String Description { get; set; }

        public double AmountRequested { get; set; }

        public PostStatus PostStatus { get; set; }

        public Guid HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public Guid BloodTypeId { get; set; }

        public BloodType BloodType { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();
    }
}
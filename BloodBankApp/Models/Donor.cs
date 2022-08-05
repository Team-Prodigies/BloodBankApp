using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Donor
    {
        [Key]
        public Guid DonorId { get; set; }

        public virtual User User { get; set; }
        public long PersonalNumber { get; set; }

        public Guid HealthFormQuestionnaireId { get; set; }

        public HealthFormQuestionnaire HealthFormQuestionnaire { get; set; }

        public Guid BloodTypeId { get; set; }

        public BloodType BloodType { get; set; }

        public Guid CityId { get; set; }

        public City City { get; set; }

        public ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();

      //  public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}

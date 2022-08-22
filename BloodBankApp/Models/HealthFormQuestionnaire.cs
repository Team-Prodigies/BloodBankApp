using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class HealthFormQuestionnaire
    {
        [Key]
        public Guid HealthFormQuestionnaireId { get; set; }
        [Display(Name = "Last updated")]
        public DateTime LastUpdated { get; set; }
        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}

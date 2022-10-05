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
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}

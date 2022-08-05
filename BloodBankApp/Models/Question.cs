using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        public String Description { get; set; }

        public Guid HealthFormQuestionnaireId { get; set; }

        public HealthFormQuestionnaire HealthFormQuestionnaire { get; set; }
    }
}

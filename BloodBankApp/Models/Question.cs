using BloodBankApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Display(Name = "Answer")]
        public Answer Answer { get; set; }
        public Guid HealthFormQuestionnaireId { get; set; }
        public HealthFormQuestionnaire HealthFormQuestionnaire { get; set; }
    }
}
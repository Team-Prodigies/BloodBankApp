using BloodBankApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Areas.Donator.ViewModels {
    public class QuestionViewModel 
    {
        [Key]
        public Guid QuestionId { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        [Display(Name = "Answer")]
        public Answer Answer { get; set; }
    }
}
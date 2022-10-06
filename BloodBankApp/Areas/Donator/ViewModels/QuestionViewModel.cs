﻿using BloodBankApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
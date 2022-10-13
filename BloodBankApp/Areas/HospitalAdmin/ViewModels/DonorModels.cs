using BloodBankApp.Enums;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class DonorModels
    {
        [Key]
        public Guid DonorId { get; set; }
        public virtual User User { get; set; }

        [Display(Name = "Personal number")]
        public long PersonalNumber { get; set; }
        public Guid? HealthFormQuestionnaireId { get; set; }
        public Gender Gender { get; set; }
        public HealthFormQuestionnaire HealthFormQuestionnaire { get; set; }
        public Guid? BloodTypeId { get; set; }
        public BloodType BloodType { get; set; }
        public Guid CityId { get; set; }
        public City City { get; set; }
        public Code Code { get; set; }
    }
}

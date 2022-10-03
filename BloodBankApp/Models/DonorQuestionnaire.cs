using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Models {
    public class DonorQuestionnaire 
    {
        [Key]
        public Guid DonorQustionnaireId { get; set; }
        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }
        public Guid HealthFormQuestionnaireId { get; set; }
        public HealthFormQuestionnaire HealthFormQuestionnaire { get; set; }
    }
}

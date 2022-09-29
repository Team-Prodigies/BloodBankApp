using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Code
    {
        [Key]
        public Guid CodeId { get; set; }

        [Required]
        public string CodeValue { get; set; }
        public Donor Donor { get; set; }
    }
}

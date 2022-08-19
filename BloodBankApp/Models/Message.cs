using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; }
        [Display(Name = "Date sent")]
        public DateTime DateSent { get; set; }
        [Required]
        [StringLength(2000)]
        public string Content { get; set; }
        public Guid DonorId { get; set; }
        public Donor Sender { get; set; }
        public Guid MedicalStaffId { get; set; }
        public MedicalStaff MedicalStaff { get; set; }
    }
}

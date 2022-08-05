using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class Message
    {
        [Key]
        public Guid MessageId { get; set; }

        public DateTime DateSent { get; set; }

        public String Content { get; set; }

        public Guid DonorId { get; set; }

        public Donor Sender { get; set; }

        public Guid MedicalStaffId { get; set; }

        public MedicalStaff MedicalStaff { get; set; }
    }
}

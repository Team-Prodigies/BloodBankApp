using BloodBankApp.Enums;
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
        public bool Seen { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; }

        public Guid DonorId { get; set; }
        public Donor Donor { get; set; }

        public Guid HospitalId { get; set; }
        public Hospital Hospital { get; set; }

        public MessageSender Sender { get; set; }

        public Message(){}
        public Message(DateTime DateSent,string Content, Guid DonorId, Guid HospitalId, int Sender)
        {
            this.DateSent = DateSent;
            this.Content = Content;
            this.DonorId = DonorId;
            this.HospitalId = HospitalId;
            this.Sender = (MessageSender)Sender;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public Guid? SenderId { get; set; }
        public User Sender { get; set; }

        public Guid? ReceiverId { get; set; }
        public User Receiver { get; set; }

    }
}

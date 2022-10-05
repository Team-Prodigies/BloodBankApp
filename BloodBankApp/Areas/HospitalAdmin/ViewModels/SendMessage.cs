using BloodBankApp.Enums;
using System;

namespace BloodBankApp.Areas.HospitalAdmin.ViewModels
{
    public class SendMessage
    {
        public Guid MessageId { get; set; }
        public bool Seen { get; set; }
        public string Content { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public Guid DonorId { get; set; }
        public Guid HospitalId { get; set; }
        public MessageSender Sender { get; set; }
    }
}
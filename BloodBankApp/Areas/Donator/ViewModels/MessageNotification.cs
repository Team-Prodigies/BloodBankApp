using BloodBankApp.Models;
using System;

namespace BloodBankApp.Areas.Donator.ViewModels
{
    public class MessageNotification
    {
        public Guid HospitalId { get; set; }
        public string HospitalName { get; set; }
    }
}

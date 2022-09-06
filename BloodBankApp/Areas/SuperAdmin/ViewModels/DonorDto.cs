using System;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class DonorDto
    {
        public Guid DonorId { get; set; }
        public long PersonalNumber { get; set; }
        public Gender Gender { get; set; }
        public Guid BloodTypeId { get; set; }
        public string BloodTypeName { get; set; }
        public Guid CityId { get; set; }
    }
}

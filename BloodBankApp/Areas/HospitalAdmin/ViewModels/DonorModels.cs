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
        public Donor Donor { get; set; }
        public int DonationsCount { get; set; }
    }
}

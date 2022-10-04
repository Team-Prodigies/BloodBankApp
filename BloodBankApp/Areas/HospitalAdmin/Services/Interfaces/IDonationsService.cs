﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IDonationsService
    {
        Task<List<BloodDonationModel>> GetAllDonations();
        Task<bool> AddDonation(BloodDonationModel donation);
        Task UpdateDonation(BloodDonationModel donation);
    }
}
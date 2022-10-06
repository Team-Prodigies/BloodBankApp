using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IDonationsService
    {
        Task<List<BloodDonationModel>> GetAllDonations(string? searchTerm);
        Task<bool> AddDonation(BloodDonationModel donation);
        Task<bool> UpdateDonation(BloodDonationModel donation);
        Task<BloodDonationModel> GetDonation(Guid donationId);
    }
}
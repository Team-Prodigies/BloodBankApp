using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IDonationsService
    {
        Task<List<BloodDonationModel>> GetAllDonations(string? searchTerm);
        Task<List<DonationRequests>> GetAllDonationRequests();
        Task<bool> AddDonation(BloodDonationModel donation);
        Task<bool> UpdateDonation(BloodDonationModel donation);
        Task<BloodDonationModel> GetDonation(Guid donationId);
        Task<bool> ApproveDonationRequest(Guid requestId, double amount);
        Task<bool> RemoveDonationRequest(Guid requestId);
        Task<Guid> GetCurrentHospitalId();
        Task<bool> AddDonationRequest(Guid postId, Guid donorId);
    }
}
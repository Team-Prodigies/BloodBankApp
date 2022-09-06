using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IDonorsService
    {
        Task<List<DonorModel>> GetDonors(int pageNumber = 1, string filterBy = "A-Z");
        Task<List<DonorModel>> DonorSearchResults(string searchTerm, int pageNumber = 1);
        Task LockoutDonor(User user);
        Task UnlockDonor(User user);
        Task<DonorDto> GetDonor(Guid donorId);
        Task EditDonor(Guid donorId, DonorDto donorDto);
        Task AddDonor(Donor donor);
    }
}

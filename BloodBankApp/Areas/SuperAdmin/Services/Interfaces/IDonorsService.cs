using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Pages.Account.Manage;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IDonorsService
    {
        Task<List<DonorModel>> GetDonors(int pageNumber = 1, string filterBy = "A-Z");
        Task<List<DonorModel>> DonorSearchResults(string searchTerm, int pageNumber = 1);
        Task LockoutDonor(User user);
        Task UnlockDonor(User user);
        Task<Donor> GetDonor(Guid donorId);
        List<Gender> GetGenders();
        Task<bool> EditDonor(Guid donorId, PersonalProfileIndexModel.ProfileInputModel donorDto);
        Task AddDonor(Donor donor);
    }
}

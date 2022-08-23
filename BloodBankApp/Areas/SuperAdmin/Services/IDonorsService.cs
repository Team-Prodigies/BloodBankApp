using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public interface IDonorsService
    {
        Task<List<DonorModel>> GetDonors(int pageNumber = 1, string filterBy = "A-Z");
        Task<List<DonorModel>> DonorSearchResults(string searchTerm, int pageNumber = 1);
        Task LockoutDonor(User user);
        Task UnlockDonor(User user);

        

    }

}

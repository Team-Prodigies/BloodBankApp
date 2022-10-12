using BloodBankApp.Areas.Donator.ViewModels;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IDonatorService
    {
        Task<List<DonatorModel>> GetDonators();
        Task<List<Donor>> FindPotencialDonors(BloodType bloodType, City city);
        Task<bool> AddNotRegisteredDonor(NotRegisteredDonor notRegisteredDonor);
        Task<bool> CodeExists(string codeValue);
        Task<List<BloodDonationsModel>> GetBloodDonationsHistory();
    }
}
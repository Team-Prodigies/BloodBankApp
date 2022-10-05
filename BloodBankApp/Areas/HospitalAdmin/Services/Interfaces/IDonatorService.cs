using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IDonatorService
    {
        Task<List<DonatorModel>> GetDonators();
        Task<bool> AddNotRegisteredDonor(NotRegisteredDonor notRegisteredDonor);
        Task<bool> CodeExists(string codeValue);
    }
}
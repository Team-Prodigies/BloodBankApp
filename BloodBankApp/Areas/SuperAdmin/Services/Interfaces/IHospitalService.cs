using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IHospitalService
    {
        Task<List<Hospital>> GetHospitals(int pageNumber);
        Task<bool> CreateHospital(HospitalModel model);
        Task EditHospital(HospitalModel hospital);
        Task<Hospital> GetHospital(Guid hospitalId);
        Task<List<Hospital>> HospitalSearchResults(string searchTerm, int pageNumber);
    }
}

using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IHospitalService
    {
        Task<List<HospitalModel>> GetHospitals(int pageNumber);
        Task<List<HospitalModel>> GetAllHospitals();
        Task<bool> CreateHospital(HospitalModel model);
        Task EditHospital(HospitalModel hospital);
        Task<HospitalModel> GetHospital(Guid hospitalId);
        Task<List<HospitalModel>> HospitalSearchResults(string searchTerm, int pageNumber);
        Task<string> GetHospitalCode(Guid hospitalId);
        Task<bool> HospitalCodeExists(string hospitalCode);
        Task<List<MedicalStaffModel>> GetAllHospitalAdminsByHospitalId(Guid hospitalId);
    }
}

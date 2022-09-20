using BloodBankApp.Areas.SuperAdmin.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IHospitalService
    {
        Task<EditHospitalModel> GetHospitalForHospitalAdmin(ClaimsPrincipal principal);
        Task<EditHospitalModel> GetHospitalForHospitalAdm(Guid hospitalId);

        Task EditHospitalForHospitalAdmin( EditHospitalModel hospital);
        Task<List<HospitalModel>> GetHospitals(int pageNumber);
        Task<List<HospitalModel>> GetAllHospitals();
        Task<List<Location>> GetAllLocations();
        Task<bool> CreateHospital(HospitalModel model);
        Task EditHospital(HospitalModel hospital);
        Task<HospitalModel> GetHospital(Guid hospitalId);
        Task<List<HospitalModel>> HospitalSearchResults(string searchTerm, int pageNumber);
        Task<string> GetHospitalCode(Guid hospitalId);
        Task<bool> HospitalCodeExists(string hospitalCode);
        Task<List<MedicalStaffModel>> GetAllHospitalAdminsByHospitalId(Guid hospitalId);
    }
}

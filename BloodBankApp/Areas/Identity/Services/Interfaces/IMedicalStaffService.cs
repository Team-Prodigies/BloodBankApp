using BloodBankApp.Models;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Identity.Services.Interfaces
{
    public interface IMedicalStaffService
    {
        Task AddMedicalStaff(MedicalStaff medicalStaff);
    }
}

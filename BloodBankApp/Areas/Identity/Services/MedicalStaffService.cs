using BloodBankApp.Data;
using BloodBankApp.Models;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Services.Interfaces;

namespace BloodBankApp.Areas.Identity.Services
{
    public class MedicalStaffService : IMedicalStaffService
    {
        private readonly ApplicationDbContext _context;

        public MedicalStaffService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddMedicalStaff(MedicalStaff medicalStaff)
        {
            await _context.MedicalStaffs.AddAsync(medicalStaff);
            await _context.SaveChangesAsync();
        }
    }
}

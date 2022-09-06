using BloodBankApp.Data;
using BloodBankApp.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HospitalCodeIsTaken(string hospitalCode)
        {
            var hospitalCodeInUse = await _context.Hospitals
                .FirstOrDefaultAsync(hospital => hospital.HospitalCode == hospitalCode);
            if (hospitalCodeInUse != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PersonalNumberIsTaken(int personalNumber)
        {
            var personalNumberInUse = await _context.Donors
                .FirstOrDefaultAsync(donor => donor.PersonalNumber == personalNumber);
            if (personalNumberInUse != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UsernameIsTaken(string username)
        {
            var personalNumberInUse = await _context.Users
                .FirstOrDefaultAsync(user => user.NormalizedUserName == username.ToUpper());
            if (personalNumberInUse != null)
            {
                return true;
            }
            return false;
        }
    }
}

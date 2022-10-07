using BloodBankApp.Data;
using BloodBankApp.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<bool> DonorCodeIsTaken(string codeValue)
        {
            var codeIsTaken = await _context.Codes
                .FirstOrDefaultAsync(c => c.CodeValue == codeValue);
            if (codeIsTaken != null)
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

        public async Task<bool> PhoneNumberIsTaken(string phoneNumber)
        {
            var phoneNo = phoneNumber;
            if (!phoneNumber.StartsWith("0"))
            {
                var newPhoneNo = phoneNo.Substring(1);
                phoneNo = newPhoneNo.Insert(0, "+");
            }
            var phoneNumberInUse = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNo);

            if (phoneNumberInUse != null) return true;
            return false;
        }

        public async Task<bool> PersonalNumberIsTaken(Guid id, long personalNumber)
        {
            var donor = await _context.Donors
                .Where(u => u.DonorId != id)
                .FirstOrDefaultAsync(d => d.PersonalNumber == personalNumber);

            if (donor != null) return true;
            return false;
        }

        public async Task<bool> PhoneNumberIsTaken(Guid id, string phoneNumber)
        {
            var phoneNo = phoneNumber;
            if (!phoneNumber.StartsWith("0"))
            {
                var newPhoneNo = phoneNo.Substring(1);
                phoneNo = newPhoneNo.Insert(0, "+");
            }
            var phoneNumberInUse = await _context.Users
                .Where(u => u.Id != id)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNo);

            if (phoneNumberInUse != null) return true;
            return false;
        }
    }
}
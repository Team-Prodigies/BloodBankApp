using BloodBankApp.Data;
using BloodBankApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ApplicationDbContext _context;

        public AvailabilityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool HospitalCodeIsTaken(string hospitalCode)
        {
            var hospitalCodeInUse = _context.Hospitals.Where(hospital => hospital.HospitalCode == hospitalCode).FirstOrDefault();
            if(hospitalCodeInUse != null)
            {
                return true;
            }
            return false;
        }
         
        public bool PersonalNumberIsTaken(int personalNumber)
        {
            var personalNumberInUse = _context.Donors.Where(donor => donor.PersonalNumber == personalNumber).FirstOrDefault();
            if(personalNumberInUse != null)
            {
                return true;
            }
            return false;
        }

        public bool UsernameIsTaken(string username)
        {
            var personalNumberInUse = _context.Users.Where(user => user.NormalizedUserName == username.ToUpper()).FirstOrDefault();
            if (personalNumberInUse != null) {
                return true;
            }
            return false;
        }
    }
}

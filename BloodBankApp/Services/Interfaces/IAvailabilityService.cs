using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IAvailabilityService
    {
        bool UsernameIsTaken(string username);
        bool PersonalNumberIsTaken(int personalNumber);
        bool HospitalCodeIsTaken(string hospitalCode);
    }
}

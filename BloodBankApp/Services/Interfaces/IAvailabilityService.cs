using System;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<bool> UsernameIsTaken(string username);
        Task<bool> PersonalNumberIsTaken(int personalNumber);
        Task<bool> PersonalNumberIsTaken(Guid id, int personalNumber);

        Task<bool> HospitalCodeIsTaken(string hospitalCode);
        Task<bool> DonorCodeIsTaken(string codeValue);
        Task<bool> PhoneNumberIsTaken(string phoneNumber);
        Task<bool> PhoneNumberIsTaken(Guid id, string phoneNumber);

    }
}
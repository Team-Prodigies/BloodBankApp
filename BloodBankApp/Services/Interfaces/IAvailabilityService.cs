using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<bool> UsernameIsTaken(string username);
        Task<bool> PersonalNumberIsTaken(int personalNumber);
        Task<bool> HospitalCodeIsTaken(string hospitalCode);
        Task<bool> DonorCodeIsTaken(string codeValue);
        Task<bool> PhoneNumberIsTaken(string phoneNumber);
    }
}
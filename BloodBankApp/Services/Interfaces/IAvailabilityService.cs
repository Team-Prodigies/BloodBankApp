namespace BloodBankApp.Services.Interfaces
{
    public interface IAvailabilityService
    {
        bool UsernameIsTaken(string username);
        bool PersonalNumberIsTaken(int personalNumber);
        bool HospitalCodeIsTaken(string hospitalCode);
    }
}

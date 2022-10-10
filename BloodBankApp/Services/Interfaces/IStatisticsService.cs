using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetNumberOfDonationPostsAsync();
        Task<int> GetAmountOfBloodDonatedAsync();
        Task<int> GetDonorCountAsync();
        Task<int> GetUsersCountAsync();
        Task<int> GetHospitalsCountAsync();
        Task<Dictionary<string, int>> GetUserRoleDataAsync();
        Task<Dictionary<string, int>> GetUserBloodDataAsync();
    }
}
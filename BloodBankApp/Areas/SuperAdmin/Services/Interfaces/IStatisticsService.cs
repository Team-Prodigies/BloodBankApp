using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetNumberOfDonationPostsAsync();
        Task<double> GetAmountOfBloodDonatedAsync();
        Task<int> GetDonorCountAsync();
        Task<int> GetUsersCountAsync();
        Task<int> GetHospitalsCountAsync();
        Task<Dictionary<string, int>> GetUserRoleDataAsync();
        Task<Dictionary<string, int>> GetUserBloodDataAsync();
    }
}

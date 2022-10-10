using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Services.Interfaces
{
    public interface ISuggestionsService
    {
        Task<IEnumerable<string>> GetDonorsSuggestionsAsync(string search);
        Task<IEnumerable<string>> GetHospitalsSuggestionsAsync(string search);
    }
}
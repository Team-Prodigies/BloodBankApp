using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.API_s
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ISuggestionsService _suggestionsService;

        public SuggestionsController(ISuggestionsService suggestionsService)
        {
            _suggestionsService = suggestionsService;
        }

        [HttpGet]
        [Route("GetDonorsSuggestions")]
        public async Task<IEnumerable<string>> GetDonorsSuggestionsAsync(string search)
        {
            return await _suggestionsService.GetDonorsSuggestionsAsync(search);
        }

        [HttpGet]
        [Route("GetHospitalsSuggestions")]
        public async Task<IEnumerable<string>> GetHospitalsSuggestionsAsync(string search)
        {
            return await _suggestionsService.GetHospitalsSuggestionsAsync(search);
        }
    }
}
using System.Threading.Tasks;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.API_s.SuperAdminAPI_s
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet]
        [Route("UsernameIsTaken")]
        public async Task<bool> UsernameIsTaken(string username)
        {
            if (username == null || username.Trim() == "")
            {
                return false;
            }
            return await _availabilityService.UsernameIsTaken(username);
        }

        [HttpGet]
        [Route("PersonalNumberIsTaken")]
        public async Task<bool> PersonalNumberIsTaken(int personalNumber)
        {
            return await _availabilityService.PersonalNumberIsTaken(personalNumber);
        }

        [HttpGet]
        [Route("HospitalCodeIsTaken")]
        public async Task<bool> HospitalCodeIsTaken(string hospitalCode)
        {
            if (hospitalCode == null || hospitalCode.Trim() == "")
            {
                return false;
            }
            return await _availabilityService.HospitalCodeIsTaken(hospitalCode);
        }

        [HttpGet]
        [Route("DonorCodeIsTaken")]
        public async Task<bool> DonorCodeIsTaken(string codeValue)
        {
            if (codeValue == null || codeValue.Trim() == "")
            {
                return false;
            }
            return await _availabilityService.DonorCodeIsTaken(codeValue);
        }
    }
}

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
        public bool UsernameIsTaken(string username)
        {
            if (username == null || username.Trim() == "")
            {
                return false;
            }
            return _availabilityService.UsernameIsTaken(username);
        }

        [HttpGet]
        [Route("PersonalNumberIsTaken")]
        public bool PersonalNumberIsTaken(int personalNumber)
        {
            return _availabilityService.PersonalNumberIsTaken(personalNumber);
        }

        [HttpGet]
        [Route("HospitalCodeIsTaken")]
        public bool HospitalCodeIsTaken(string hospitalCode)
        {
            if (hospitalCode == null || hospitalCode.Trim() == "")
            {
                return false;
            }
            return _availabilityService.HospitalCodeIsTaken(hospitalCode);
        }
    }
}

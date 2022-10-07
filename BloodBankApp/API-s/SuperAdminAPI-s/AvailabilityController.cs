using System;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankApp.API_s.SuperAdminAPI_s
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersService _usersService;

        public AvailabilityController(IAvailabilityService availabilityService,
            IHttpContextAccessor httpContextAccessor,
            IUsersService usersService)
        {
            _availabilityService = availabilityService;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
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
        [HttpGet]
        [Route("PhoneNumberIsTaken")]
        public async Task<bool> PhoneNumberIsTaken(string phoneNumber)
        {
            return await _availabilityService.PhoneNumberIsTaken(phoneNumber);
        }
        [HttpGet]
        [Route("PersonalNumberIsTakenApi")]
        public async Task<bool> PersonalNumberIsTakenApi(long personalNumber)
        {
            var user = await _usersService.GetUser(User);
          //  Guid id = user.Id;
            return await _availabilityService.PersonalNumberIsTaken(user.Id, personalNumber);
        }
        [HttpGet]
        [Route("PhoneNumberIsTakenApi")]
        public async Task<bool> PhoneNumberIsTakenApi(string phoneNumber)
        {
            var user = await _usersService.GetUser(User);
            var id = user.Id;
            return await _availabilityService.PhoneNumberIsTaken(id,phoneNumber);
        }
    }
}

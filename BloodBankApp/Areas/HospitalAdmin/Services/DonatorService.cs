using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class DonatorService : IDonatorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUsersService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonatorService(ApplicationDbContext context, IMapper mapper, IUsersService usersService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userService = usersService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DonatorModel>> GetDonators()
        {
            var hospitalId = await _context.MedicalStaffs.Where(hospitalAdmin => hospitalAdmin.MedicalStaffId == _userService.GetUser(_httpContextAccessor.HttpContext.User).Result.Id).Select(hospitalAdmin => hospitalAdmin.HospitalId).FirstOrDefaultAsync();
            
            var donators = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .Include(bloodDonations => bloodDonations.BloodDonations)
                            .ThenInclude(donationPost => donationPost.DonationPost)
                        .Where(donators => (donators.BloodDonations.Count != 0) && (donators.BloodDonations.Any(bloodDonation => bloodDonation.DonationPost.HospitalId == hospitalId)))
                        .ToListAsync();

            var result = _mapper.Map<List<DonatorModel>>(donators);

            return result;
        }
    }
}

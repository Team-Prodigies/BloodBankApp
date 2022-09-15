using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
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

        public DonatorService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DonatorModel>> GetDonators()
        {
            List<Donor> donators;
            donators = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .Where(donators => (donators.BloodDonations.Count != 0))
                        .ToListAsync();
            var result = _mapper.Map<List<DonatorModel>>(donators);

            return result;
        }
    }
}

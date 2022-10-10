using System;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.ExtensionMethods;
using BloodBankApp.Models;
using BloodBankApp.Areas.Donator.ViewModels;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class DonatorService : IDonatorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUsersService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonatorService(ApplicationDbContext context,
            IMapper mapper,
            IUsersService usersService,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userService = usersService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<DonatorModel>> GetDonators()
        {
            var hospitalId = await _context.MedicalStaffs
                .Where(hospitalAdmin => hospitalAdmin.MedicalStaffId == _userService.GetUser(_httpContextAccessor.HttpContext.User).Result.Id)
                .Select(hospitalAdmin => hospitalAdmin.HospitalId).FirstOrDefaultAsync();

            var donators = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .Include(bloodDonations => bloodDonations.BloodDonations)
                            .ThenInclude(donationPost => donationPost.DonationPost)
                        .Where(donators => (donators.BloodDonations.Count != 0)
                        && (donators.BloodDonations.Any(bloodDonation => bloodDonation.DonationPost.HospitalId == hospitalId)))
                        .ToListAsync();

            var result = _mapper.Map<List<DonatorModel>>(donators);
            return result;
        }

        public async Task<bool> AddNotRegisteredDonor(NotRegisteredDonor notRegisteredDonor)
        {
            notRegisteredDonor.Name = notRegisteredDonor.Name.ToTitleCase();
            notRegisteredDonor.Surname = notRegisteredDonor.Surname.ToTitleCase();

            await using var transaction = await _context.Database.BeginTransactionAsync();

            var user = _mapper.Map<User>(notRegisteredDonor);
            user.Id = Guid.NewGuid();
            var donor = _mapper.Map<Donor>(notRegisteredDonor);

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                donor.DonorId = user.Id;

                await _context.Donors.AddAsync(donor);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return false;
            }
            return true;
        }

        public async Task<bool> CodeExists(string codeValue)
        {
            var code = await _context.Codes
                .FirstOrDefaultAsync(c => c.CodeValue == codeValue);

            if (code != null) return true;
            return false;
        }

        public async Task<List<BloodDonationsModel>> GetBloodDonationsHistory()
        {
            var user = await _userService.GetUser(_httpContextAccessor.HttpContext.User);
            var bloodDonation = await _context.BloodDonations
                .Include(x => x.Hospital)
                .Include(x => x.Donor)
                .Include(x => x.DonationPost)
                .Where(x => x.DonorId == user.Id)
                .ToListAsync();
            var result = _mapper.Map<List<BloodDonationsModel>>(bloodDonation);

            return result;
        }
    }
}
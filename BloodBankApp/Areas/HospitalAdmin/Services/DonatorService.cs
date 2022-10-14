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
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Services;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class DonatorService : IDonatorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUsersService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHospitalService _hospitalService;
        private readonly INotificationService _notificationService;
        private readonly IBloodTypesService _bloodTypesService;

        public DonatorService(ApplicationDbContext context,
            IMapper mapper,
            IUsersService usersService,
            IHttpContextAccessor httpContextAccessor,
            IHospitalService hospitalService,
            INotificationService notificationService,
            IBloodTypesService bloodTypesService)
        {
            _context = context;
            _mapper = mapper;
            _userService = usersService;
            _httpContextAccessor = httpContextAccessor;
            _hospitalService = hospitalService;
            _notificationService = notificationService;
            _bloodTypesService = bloodTypesService;
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

        public async Task<List<DonorModels>> FindPotencialDonors(Guid bloodTypeId, Guid cityId)
        {
            var bloodType = await _bloodTypesService.GetBloodType(bloodTypeId);
            var potencialDonors = await _notificationService.GetPotentialDonors(bloodType.BloodTypeName, cityId);

            var donorDonation = new List<DonorModels>();

            foreach(var donor in potencialDonors)
            {
                donorDonation.Add(new DonorModels { Donor = donor, DonationsCount = donor.BloodDonations.Count });
            }

            donorDonation = donorDonation.OrderBy(donation => donation.DonationsCount).Take(15).ToList();
            
            return donorDonation;
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
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }

            var user = await _userService.GetUser(_httpContextAccessor.HttpContext.User);
            var bloodDonation = await _context.BloodDonations
                .Include(hospital => hospital.Hospital)
                .Include(donor => donor.Donor)
                .Where(x => x.DonorId == user.Id)
                .ToListAsync();
            var result = _mapper.Map<List<BloodDonationsModel>>(bloodDonation);

            return result;

        }
    }
}
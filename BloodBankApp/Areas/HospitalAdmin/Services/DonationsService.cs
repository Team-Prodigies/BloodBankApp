using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class DonationsService:IDonationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUsersService _usersService;
        public DonationsService(ApplicationDbContext context, 
            IMapper mapper,
            IHttpContextAccessor contextAccessor, 
            IUsersService usersService)
        {
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _usersService = usersService;
        }

        private async Task<Guid> GetCurrentHospitalId()
        {
            var user = await _usersService.GetUser(_contextAccessor.HttpContext.User);
            var hospitalAdmin = await _context.MedicalStaffs.Where(staff => staff.MedicalStaffId == user.Id).FirstOrDefaultAsync();
            return await _context.Hospitals.Where(hospital => hospital.HospitalId == hospitalAdmin.HospitalId).Select(hospital => hospital.HospitalId).FirstOrDefaultAsync();
        }
        public async Task<List<BloodDonationModel>> GetAllDonations()
        {
            var hospitalId = await GetCurrentHospitalId(); 
            var donations = await _context.BloodDonations
                .Include(donation => donation.Hospital)
                .Include(donation => donation.Donor)
                    .ThenInclude(donor => donor.User)
                .Where(donation => donation.HospitalId == hospitalId)
                .ToListAsync();
            var result = _mapper.Map<List<BloodDonationModel>>(donations);
            return result;
        }

        public async Task<bool> AddDonation(BloodDonationModel donation)
        {
            try
            {
                var hospitalId = await GetCurrentHospitalId();
                var bloodDonation = _mapper.Map<BloodDonation>(donation);
                bloodDonation.HospitalId = hospitalId;
                bloodDonation.DonorId = donation.Donor.DonorId;
                bloodDonation.Donor = null;
                bloodDonation.DonationPostId = null;
                await _context.BloodDonations.AddAsync(bloodDonation);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public Task UpdateDonation(BloodDonationModel donation)
        {
            throw new System.NotImplementedException();
        }
    }
}
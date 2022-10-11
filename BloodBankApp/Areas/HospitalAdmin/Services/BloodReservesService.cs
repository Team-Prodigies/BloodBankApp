using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.HospitalAdmin.Services
{
    public class BloodReservesService : IBloodReservesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDonationsService _donationsService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public BloodReservesService(ApplicationDbContext context,
            IDonationsService donationsService,
            IMapper mapper, 
            INotyfService notyfService)
        {
            _context = context;
            _donationsService = donationsService;
            _mapper = mapper;
            _notyfService = notyfService;
        }

        public async Task<List<BloodReserveModel>> GetBloodReserves()
        {
            var hospitalId = await _donationsService.GetCurrentHospitalId();
            var reserves = await _context.BloodReserves
                .Include(reserve => reserve.BloodType)
                .Where(reserve => reserve.HospitalId == hospitalId)
                .ToListAsync();
            var result = new List<BloodReserveModel>();
            foreach (var reserve in reserves)
            {
                var bloodReserve = _mapper.Map<BloodReserveModel>(reserve);
                bloodReserve.BloodType = reserve.BloodType.BloodTypeName;
                result.Add(bloodReserve);
            }
            return result;
        }

        public async Task<BloodReserveModel> GetBloodReserve(Guid reserveId)
        {
            var reserve = await _context.BloodReserves
                .Include(reserve => reserve.BloodType)
                .Where(reserve => reserve.BloodReserveId == reserveId)
                .FirstOrDefaultAsync();
            return _mapper.Map<BloodReserveModel>(reserve);
        }

        public async Task<bool> SetBloodReserve(BloodReserveModel model)
        {
            var hospitalId = await _donationsService.GetCurrentHospitalId();
            var bloodReserve = _mapper.Map<BloodReserve>(model);
            bloodReserve.HospitalId = hospitalId;
            if (bloodReserve.BloodReserveId == Guid.Empty)
            {
                var bloodReserveExists = await _context.BloodReserves
                    .Include(reserve => reserve.BloodType)
                    .Where(reserve => reserve.HospitalId == hospitalId)
                    .AnyAsync(reserve => reserve.BloodType.BloodTypeId == model.BloodTypeId);
                if (bloodReserveExists)
                {
                    _notyfService.Warning($"Blood reserve for selected blood type already exists" );
                    return false;
                }
                await _context.BloodReserves.AddAsync(bloodReserve);
            }
            else
            {
                var dbReserve = await _context.BloodReserves.FindAsync(bloodReserve.BloodReserveId);
                dbReserve.Amount = bloodReserve.Amount;
                dbReserve.BloodTypeId = bloodReserve.BloodTypeId;
                _context.BloodReserves.Update(dbReserve);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            if (model.Amount < 10)
            {
                var potentialDonors = await GetPotentialDonors(model, hospitalId);
                //TODO: Send notification to potentialDonors
            }
            return true;
        }

        private async Task<List<Donor>> GetPotentialDonors(BloodReserveModel reserve, Guid hospitalId)
        {
            var cityId = await _context.Hospitals.Where(hospital => hospital.HospitalId == hospitalId).Select(hospital => hospital.CityId).FirstOrDefaultAsync();

            //TODO: Change "donor.BloodTypeId == reserve.BloodTypeId" to take into account any blood type compatible with the required bloodType
            var potentialDonors = await _context.Donors
                .Include(donor => donor.BloodDonations)
                .Where(donor => donor.BloodTypeId == reserve.BloodTypeId 
                                && donor.CityId == cityId)
                .ToListAsync();

            potentialDonors = potentialDonors.Where(donor => donor.Gender == Gender.FEMALE
                ? donor.BloodDonations.All(donation => (DateTime.Now - donation.DonationDate).Days > 120)
                : donor.BloodDonations.All(donation => (DateTime.Now - donation.DonationDate).Days > 90))
                .ToList();
            return potentialDonors;
        }
    }
}

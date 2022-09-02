using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public HospitalService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateHospital(HospitalModel model)
        {
            var location = _mapper.Map<Location>(model);
            await _context.AddAsync(location);

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    var hospital = _mapper.Map<Hospital>(model);
                    hospital.LocationId = location.LocationId;
                    await _context.AddAsync(hospital);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }

        public async Task EditHospital(HospitalModel hospital)
        {
            var editHospital = _mapper.Map<Hospital>(hospital);

            _context.Update(editHospital.Location);
            _context.Update(editHospital);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Hospital>> GetAllHospitals()
        {
            return await _context.Hospitals.ToListAsync();
        }

        public async Task<Hospital> GetHospital(Guid hospitalId)
        {
            return await _context.Hospitals
                .Include(l => l.Location)
                .Include(c => c.City)
                .Where(h => h.HospitalId == hospitalId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Hospital>> GetHospitals(int pageNumber)
        {
            var skipRows = (pageNumber - 1) * 10;
            return await _context.Hospitals
                .Include(c => c.City)
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();
        }

        public async Task<bool> HospitalCodeExists(string hospitalCode)
        {
            var hospitalCodeInUse = await _context.Hospitals
                .Where(hospital => hospital.HospitalCode == hospitalCode)
                .FirstOrDefaultAsync();

            if(hospitalCodeInUse != null)
            {
                return true;
            }
            return false; 
        }

        public async Task<List<Hospital>> HospitalSearchResults(string searchTerm, int pageNumber)
        {
            var skipRows = (pageNumber - 1) * 10;
            return await _context.Hospitals
                .Where(hospital => hospital.HospitalName.ToUpper()
                .Contains(searchTerm.ToUpper()))
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();
        }
    }
}
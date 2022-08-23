using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services {
    public class HospitalService : IHospitalService
     {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;
        public HospitalService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task CreateHospital(HospitalModel model)
        {
            var location = mapper.Map<Location>(model);
             await _context.AddAsync(location);
             await _context.SaveChangesAsync();

            var hospital = mapper.Map<Hospital>(model);
            hospital.LocationId = location.LocationId;
             await _context.AddAsync(hospital);
             await _context.SaveChangesAsync();
        }

        public async Task EditHospital(HospitalModel hospital)
        {
            var editHospital = mapper.Map<Hospital>(hospital);

            _context.Update(editHospital.Location);
            _context.Update(editHospital);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Hospital>> GetHospitals(int pageNumber)
        {
            var skipRows = (pageNumber - 1) * 10;
            return await _context.Hospitals.Include(c => c.City).Skip(skipRows).Take(10).ToListAsync();
        }

        public async Task<List<Hospital>> HospitalSearchResults(string searchTerm, int pageNumber)
         {
            var skipRows = (pageNumber - 1) * 10;
            return await _context.Hospitals.Where(hospital => hospital.HospitalName.ToUpper().Contains(searchTerm.ToUpper())).Skip(skipRows).Take(10).ToListAsync();
        }
    }
}


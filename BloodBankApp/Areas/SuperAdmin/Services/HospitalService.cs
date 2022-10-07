using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public HospitalService(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> CreateHospital(HospitalModel model)
        {
            var location = _mapper.Map<Location>(model);
            await _context.AddAsync(location);

            await using var transaction = await _context.Database.BeginTransactionAsync();
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
                await transaction.RollbackAsync();
                return false;
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

        public async Task<List<MedicalStaffModel>> GetAllHospitalAdminsByHospitalId(Guid hospitalId)
        {
            var hospitalAdmins = await _context.MedicalStaffs
           .Where(hospital => hospital.HospitalId == hospitalId)
           .Include(user => user.User)
           .ToListAsync();

            return _mapper.Map<List<MedicalStaffModel>>(hospitalAdmins);
        }

        public async Task<List<HospitalModel>> GetAllHospitals()
        {
            var hospitals= await _context.Hospitals.ToListAsync();
            return _mapper.Map<List<HospitalModel>>(hospitals);
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            return _mapper.Map<List<Location>>(locations);
        }

        public async Task<HospitalModel> GetHospital(Guid hospitalId)
        {
            var hospital = await _context.Hospitals
                .Include(l => l.Location)
                .Include(c => c.City)
                .Where(h => h.HospitalId == hospitalId)
                .FirstOrDefaultAsync();
            return _mapper.Map<HospitalModel>(hospital);
        }

        public async Task<string> GetHospitalCode(Guid hospitalId)
        {      
            var hospital = await _context.Hospitals.AsNoTracking().FirstOrDefaultAsync(hospital => hospital.HospitalId == hospitalId);
            if (hospital == null)
            {
                return null;
            }
            return hospital.HospitalCode;
        }

        public async Task<EditHospitalModel> GetHospitalForHospitalAdmin(ClaimsPrincipal principal)
        {
            var user =await _userManager.GetUserAsync(principal);

            var hospitalAdmin = await _context.MedicalStaffs
                .Include(h=>h.Hospital)
                .FirstOrDefaultAsync(x => x.MedicalStaffId == user.Id);

            var hospital = await _context.Hospitals
                .Include(c=>c.City)
                .Include(l=>l.Location)
                .FirstOrDefaultAsync(x => x.HospitalId ==hospitalAdmin.HospitalId);

            return _mapper.Map<EditHospitalModel>(hospital);
        }

        public async Task<EditHospitalModel> GetHospitalForHospitalAdm(Guid hospitalId)
        {
            var hospital = await _context.Hospitals
                .Include(l => l.Location)
                .AsNoTracking()
                .Include(c => c.City)
                .AsNoTracking()
                .Where(h => h.HospitalId == hospitalId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return _mapper.Map<EditHospitalModel>(hospital);
        }

        public async Task EditHospitalForHospitalAdmin(EditHospitalModel editHospital)
        {
            var hospital = await _context.Hospitals
                .Include(l => l.Location)
                .AsNoTracking()
                .Where(h => h.HospitalId == editHospital.HospitalId)
                .FirstOrDefaultAsync();

            hospital.HospitalName = editHospital.HospitalName;
            hospital.ContactNumber = editHospital.ContactNumber;
            hospital.Location = editHospital.Location;
            hospital.CityId = editHospital.CityId;

            _context.Update(hospital.Location);
            _context.Update(hospital);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HospitalModel>> GetHospitals(int pageNumber)
        {
            var skipRows = (pageNumber - 1) * 10;
            var hospitals= await _context.Hospitals
                .Include(c => c.City)
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();
            return _mapper.Map<List<HospitalModel>>(hospitals);
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

        public async Task<List<HospitalModel>> HospitalSearchResults(string searchTerm, int pageNumber)
        {
            var skipRows = (pageNumber - 1) * 10;
            var hospitals= await _context.Hospitals
                .Where(hospital => hospital.HospitalName.ToUpper()
                .Contains(searchTerm.ToUpper()))
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();
            return _mapper.Map<List<HospitalModel>>(hospitals);
        }

        public async Task<Hospital> GetHospitalForMedicalStaff(string currentHospitalAdminId)
        {
            var hospitalAdmin =await _context.MedicalStaffs
                .Where(ms => ms.MedicalStaffId == new Guid(currentHospitalAdminId))
                .FirstOrDefaultAsync();

            var hospital = await _context.Hospitals
                .Where(h => h.HospitalId == hospitalAdmin.HospitalId)
                .FirstOrDefaultAsync();

            return hospital;
        }
    }
}
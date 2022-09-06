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
    public class DonorsService : IDonorsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DonorsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddDonor(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DonorModel>> DonorSearchResults(string searchTerm, int pageNumber = 1)
        {
            var skipRows = (pageNumber - 1) * 10;
            var donors = await _context.Donors
                .Include(user => user.User)
                .Include(blood => blood.BloodType)
                .Include(city => city.City)
                .Where(donor => (donor.User.Name + donor.User.Surname.ToUpper())
                .Contains(searchTerm.Replace(" ", "").ToUpper()))
                .Skip(skipRows)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<DonorModel>>(donors);
        }

        public async Task EditDonor(Guid donorId, DonorDto donorDto)
        {
            var donor = await _context.Donors.FirstOrDefaultAsync(d=>d.DonorId == donorId);
            _mapper.Map(donorDto, donor);
            await _context.SaveChangesAsync();
        }

        public async Task<DonorDto> GetDonor(Guid donorId)
        {
            var donor = await _context.Donors
                .Include(c => c.City)
                .Include(b => b.BloodType)
                .FirstOrDefaultAsync(x => x.DonorId == donorId);

            var donorDto = _mapper.Map<DonorDto>(donor);
            return donorDto;
        }

        public async Task<List<DonorModel>> GetDonors(int pageNumber = 1, string filterBy = "A-Z")
        {
            var skipRows = (pageNumber - 1) * 10;
            List<Donor> donors;

            switch (filterBy)
            {
                case "A-Z":
                    donors = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .OrderBy(donor => donor.User.Name)
                        .Skip(skipRows)
                        .Take(10)
                        .ToListAsync();
                    break;

                case "Z-A":
                    donors = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .OrderByDescending(donor => donor.User.Name)
                        .Skip(skipRows)
                        .Take(10)
                        .ToListAsync();
                    break;

                case "Locked":
                    donors = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .Where(donor => donor.User.Locked == true)
                        .Skip(skipRows)
                        .Take(10)
                        .ToListAsync();
                    break;

                default:
                    donors = await _context.Donors
                        .Include(user => user.User)
                        .Include(blood => blood.BloodType)
                        .Include(city => city.City)
                        .OrderBy(donor => donor.User.Name)
                        .Skip(skipRows)
                        .Take(10)
                        .ToListAsync();
                    break;
            }
            var result = _mapper.Map<List<DonorModel>>(donors);

            return result;
        }

        public async Task LockoutDonor(User user)
        {
            user.Locked = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UnlockDonor(User user)
        {
            user.Locked = false;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}

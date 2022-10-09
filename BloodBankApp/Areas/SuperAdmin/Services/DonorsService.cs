﻿using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Pages.Account.Manage;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class DonorsService : IDonorsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public DonorsService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> PersonalNumberIsInUse(long personalNumber)
        {
            var donor = await _context.Donors
                .Where(d => d.PersonalNumber == personalNumber)
                .FirstOrDefaultAsync();
            if (donor == null) return false;
            return true;
        }

        public async Task AddDonor(Donor donor)
        {
            await _context.Donors.AddAsync(donor);
            await _context.SaveChangesAsync();
        }


        public async Task<Donor> FindDonor(long personalNumber)
        {
            var donor = await _context.Donors
                .Include(donor => donor.User)
                .Include(donor => donor.BloodType)
                .Where(donor => donor.PersonalNumber == personalNumber)
                .FirstOrDefaultAsync();
            return donor;
        }

        public List<Gender> GetGenders()
        {
           return Enum.GetValues(typeof(Gender))
               .Cast<Gender>()
               .ToList();
        }

        public async Task<bool> EditDonor(Guid donorId, PersonalProfileIndexModel.ProfileInputModel donorDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(d=>d.Id == donorId);
            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == donorId);

            _mapper.Map(donorDto, user);
            _mapper.Map(donorDto, donor);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Donor> GetDonor(Guid donorId)
        {
            var donor = await _context.Donors
                .Include(c => c.City)
                .Include(b => b.BloodType)
                .FirstOrDefaultAsync(x => x.DonorId == donorId);
            
            return donor;
        }
    }
}
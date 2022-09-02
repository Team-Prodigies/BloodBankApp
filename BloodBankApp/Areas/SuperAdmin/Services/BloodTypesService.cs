using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class BloodTypesService : IBloodTypesService
    {
        private readonly ApplicationDbContext _context;

        public BloodTypesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BloodType> GetBloodType(Guid bloodTypeId)
        {
            return await _context.BloodTypes.FindAsync(bloodTypeId);
        }

        public async Task<List<BloodType>> GetAllBloodTypes()
        {
            return await _context.BloodTypes.ToListAsync();
        }
    }
}

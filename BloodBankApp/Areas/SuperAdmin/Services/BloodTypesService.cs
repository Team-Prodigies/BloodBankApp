using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<BloodType> GetBloodType(Guid bloodTypeID)
        {
            return await _context.BloodTypes.FindAsync(bloodTypeID);
        }

        public async Task<List<BloodType>> GetAllBloodTypes()
        {
            return await _context.BloodTypes.ToListAsync();
        }
    }
}

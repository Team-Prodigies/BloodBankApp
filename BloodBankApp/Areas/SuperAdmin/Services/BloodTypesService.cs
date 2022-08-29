using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using BloodBankApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class BloodTypesService : GenericService<BloodType>, IBloodTypesService
    {
        private readonly ApplicationDbContext _context;

        public BloodTypesService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewBloodType(string bloodTypeName)
        {
            var bloodTypeExists = await _context.BloodTypes
                .Where(b => b.BloodTypeName.ToUpper() == bloodTypeName.ToUpper())
                .FirstOrDefaultAsync();

            if (bloodTypeExists == null)
            {
                BloodType newBloodType = new BloodType();
                newBloodType.BloodTypeName = bloodTypeName;

                await _context.AddAsync(newBloodType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditBloodType(BloodType editBloodType)
        {

            var bloodTypeExists = await _context.BloodTypes
                .Where(b => b.BloodTypeName.ToUpper() == editBloodType.BloodTypeName.ToUpper())
                .FirstOrDefaultAsync();

            if (bloodTypeExists == null)
            {
                var bloodType = await _context.BloodTypes.FindAsync(editBloodType.BloodTypeId);
                if (bloodType != null)
                {
                    bloodType.BloodTypeName = editBloodType.BloodTypeName;
                    _context.Update(bloodType);
                    await _context.SaveChangesAsync();
                }
            }
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

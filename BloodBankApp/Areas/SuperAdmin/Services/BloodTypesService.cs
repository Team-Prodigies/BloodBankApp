using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class BloodTypesService : IBloodTypesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BloodTypesService(ApplicationDbContext context,
        IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<BloodTypeModel>> GetAllBloodTypes()
        {
            var bloodTypes = await _context.BloodTypes.ToListAsync();

            var bloodTypeModels = _mapper.Map<List<BloodTypeModel>>(bloodTypes);

            return bloodTypeModels;
        }
    }
}

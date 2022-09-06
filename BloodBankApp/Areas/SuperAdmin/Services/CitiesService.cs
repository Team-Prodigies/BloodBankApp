using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.ExtensionMethods;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CitiesService(ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AddCity(CityModel cityModel)
        {
            var city = _mapper.Map<City>(cityModel);
            var cityExists = await _context.Cities
                .Where(b => b.CityName.ToUpper() == city.CityName.ToUpper())
                .FirstOrDefaultAsync();
            if(cityExists == null)
            {
                city.CityName = city.CityName.ToTitleCase();
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<bool> EditCity(Guid id, string cityName)
        {
            var cityExists = await _context.Cities
                .Where(b => b.CityName.ToUpper() == cityName.ToUpper())
                .FirstOrDefaultAsync();

            if (cityExists == null)
            {
                var city = await _context.Cities.FindAsync(id);
                if (city != null)
                {
                    city.CityName = cityName.ToTitleCase();
                    _context.Update(city);
                  await _context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CityModel>> GetCities()
        {
            var cities = await _context.Cities.ToListAsync();

            var cityModels = _mapper.Map<List<CityModel>>(cities);

            return cityModels;
        }

        public async Task<CityModel> GetCity(Guid id)
        {
            var city = await _context.Cities.FindAsync(id);
            var cityModel = _mapper.Map<CityModel>(city);

            return cityModel;
        }
    }
}

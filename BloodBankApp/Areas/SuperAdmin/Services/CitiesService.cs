﻿using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.ExtensionMethods;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly ApplicationDbContext _context;

        public CitiesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCity(City city)
        {
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
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<City> GetCity(Guid id)
        {
            return await _context.Cities.FindAsync(id);
        }
    }
}

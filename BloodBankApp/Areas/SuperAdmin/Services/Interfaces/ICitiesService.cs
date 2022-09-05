using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface ICitiesService
    {
        Task<IEnumerable<City>> GetCities();
        Task<bool> AddCity(City city);
        Task<bool> EditCity(Guid id, string cityName);
        Task<City> GetCity(Guid id);
    }
}

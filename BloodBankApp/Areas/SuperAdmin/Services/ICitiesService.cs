using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public interface ICitiesService
    {
        Task<IEnumerable<City>> GetCities();
        Task AddCity(City city);
        Task EditCity(Guid id ,string cityName);
        Task<City> GetCity(Guid id);
    }
}

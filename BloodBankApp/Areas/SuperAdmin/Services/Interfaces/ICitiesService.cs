using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.ViewModels;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface ICitiesService
    {
        Task<IEnumerable<CityModel>> GetCities();
        Task<bool> AddCity(CityModel city);
        Task<bool> EditCity(Guid id, string cityName);
        Task<CityModel> GetCity(Guid id);
    }
}

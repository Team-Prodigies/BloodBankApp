using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public interface IBloodTypesService
    {
        Task AddNewBloodType(string bloodTypeName);
        Task EditBloodType(BloodType editBloodType);
        Task<BloodType> GetBloodType(Guid bloodTypeID);
        Task<List<BloodType>> GetAllBloodTypes();
    }
}

using BloodBankApp.Models;
using BloodBankApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IBloodTypesService : IGenericService<BloodType>
    {
        Task AddNewBloodType(string bloodTypeName);
        Task EditBloodType(BloodType editBloodType);
        Task<BloodType> GetBloodType(Guid bloodTypeID);
        Task<List<BloodType>> GetAllBloodTypes();
    }
}

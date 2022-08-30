using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IBloodTypesService
    {
        Task<BloodType> GetBloodType(Guid bloodTypeID);
        Task<List<BloodType>> GetAllBloodTypes();
    }
}

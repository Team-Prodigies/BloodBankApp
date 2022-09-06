using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.SuperAdmin.ViewModels;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IBloodTypesService
    {
        Task<List<BloodTypeModel>> GetAllBloodTypes();
    }
}

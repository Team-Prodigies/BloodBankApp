using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces
{
    public interface IBloodReservesService
    {
        Task<List<BloodReserveModel>> GetBloodReserves();
        Task<BloodReserveModel> GetBloodReserve(Guid reserveId);
        Task<bool> SetBloodReserve(BloodReserveModel model);
    }
}

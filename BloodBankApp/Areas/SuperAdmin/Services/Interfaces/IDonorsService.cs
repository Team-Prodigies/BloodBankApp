using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Pages.Account.Manage;
using BloodBankApp.Enums;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IDonorsService
    {
        Task<Donor> GetDonor(Guid donorId);
        Task<Donor> FindDonor(long personalNumber);
        List<Gender> GetGenders();
        Task<bool> EditDonor(Guid donorId, PersonalProfileIndexModel.ProfileInputModel donorDto);
        Task<bool> PersonalNumberIsInUse(Guid id, long personalNumber);
        Task<bool> PersonalNumberIsInUse(long personalNumber);
        Task AddDonor(Donor donor);
    }
}
using BloodBankApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.Identity.Pages.Account.Manage;
using BloodBankApp.Enums;
using Microsoft.AspNetCore.Identity;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface IDonorsService
    {
        Task<Donor> GetDonor(Guid donorId);
        List<Gender> GetGenders();
        Task<bool> EditDonor(Guid donorId, PersonalProfileIndexModel.ProfileInputModel donorDto);
        Task<bool> PersonalNumberIsInUse(Guid donorId, PersonalProfileIndexModel.ProfileInputModel donorDto);
        Task AddDonor(Donor donor);
    }
}

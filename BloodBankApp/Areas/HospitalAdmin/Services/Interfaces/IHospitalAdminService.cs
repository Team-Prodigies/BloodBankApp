using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Services.Interfaces {
    public interface IHospitalAdminService {
        Task<IdentityResult> EditHospitalAdmin(HospitalAdminModel hospitalModel);
    }
}

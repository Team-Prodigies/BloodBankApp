using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    public class AdminHomeController : Controller {

        private readonly ApplicationDbContext context;
        public AdminHomeController(ApplicationDbContext context) {
            this.context = context;
        }

        public async Task<IActionResult> Index() {
            
            await GetUserRoleData();
            await GetUserBloodData();
            return View();
        }

        private async Task GetUserRoleData()
        {
            var getDonorRole = await  context.Roles.Where(x => x.Name == "Donor").FirstOrDefaultAsync();
            int? donorCount = await context.UserRoles.Where(x => x.RoleId == getDonorRole.Id).CountAsync();

            ViewBag.Donor = (donorCount == null) ? 0 : donorCount;

            var getSuperAdminRole = await context.Roles.Where(x => x.Name == "SuperAdmin").FirstOrDefaultAsync();
            int? superAdminCount = await context.UserRoles.Where(x => x.RoleId == getSuperAdminRole.Id).CountAsync();

            ViewBag.SuperAdmin = (superAdminCount == null) ? 0 : superAdminCount;

            var getHospitalAdminRole = await context.Roles.Where(x => x.Name == "HospitalAdmin").FirstOrDefaultAsync();
            int? hospitalAdminCount = await context.UserRoles.Where(x => x.RoleId == getHospitalAdminRole.Id).CountAsync();

            ViewBag.HospitalAdmin = (hospitalAdminCount == null) ? 0 : hospitalAdminCount;
        }

        private async Task GetUserBloodData()
        {
            var getAPos = await context.BloodTypes.Where(x => x.BloodTypeName == "A+").FirstOrDefaultAsync();
            int? aPosCount = await context.Donors.Where(x => x.BloodTypeId == getAPos.BloodTypeId).CountAsync();
            ViewBag.aPositive= (aPosCount == null) ? 0 : aPosCount;

            var getANeg = await context.BloodTypes.Where(x => x.BloodTypeName == "A-").FirstOrDefaultAsync();
            int? aNegCount = await context.Donors.Where(x => x.BloodTypeId == getANeg.BloodTypeId).CountAsync();
            ViewBag.aNegative = (aNegCount == null) ? 0 : aNegCount;

            var getBPos = await context.BloodTypes.Where(x => x.BloodTypeName == "B+").FirstOrDefaultAsync();
            int? bPosCount = await context.Donors.Where(x => x.BloodTypeId == getBPos.BloodTypeId).CountAsync();
            ViewBag.bPositive = (bPosCount == null) ? 0 : bPosCount;

            var getBNeg = await context.BloodTypes.Where(x => x.BloodTypeName == "B-").FirstOrDefaultAsync();
            int? bNegCount = await context.Donors.Where(x => x.BloodTypeId == getBNeg.BloodTypeId).CountAsync();
            ViewBag.bNegative = (bNegCount == null) ? 0 : bNegCount;


            var getABPos = await context.BloodTypes.Where(x => x.BloodTypeName == "AB+").FirstOrDefaultAsync();
            int? aBPosCount = await context.Donors.Where(x => x.BloodTypeId == getABPos.BloodTypeId).CountAsync();
            ViewBag.aBPositive = (aBPosCount == null) ? 0 : aBPosCount;

            var getABNeg = await context.BloodTypes.Where(x => x.BloodTypeName == "AB-").FirstOrDefaultAsync();
            int? aBNegCount = await context.Donors.Where(x => x.BloodTypeId == getABNeg.BloodTypeId).CountAsync();
            ViewBag.aBNegative = (aBNegCount == null) ? 0 : aBNegCount;

            var getOPos = await context.BloodTypes.Where(x => x.BloodTypeName == "O+").FirstOrDefaultAsync();
            int? oPosCount = await context.Donors.Where(x => x.BloodTypeId == getOPos.BloodTypeId).CountAsync();
            ViewBag.oPositive = (oPosCount == null) ? 0 : oPosCount;

            var getONeg = await context.BloodTypes.Where(x => x.BloodTypeName == "O-").FirstOrDefaultAsync();
            int? oNegCount = await context.Donors.Where(x => x.BloodTypeId == getONeg.BloodTypeId).CountAsync();
            ViewBag.oNegative = (oNegCount == null) ? 0 : oNegCount;
        }

        public IActionResult ManageCities()
        {
            return View();
        }
    }
}

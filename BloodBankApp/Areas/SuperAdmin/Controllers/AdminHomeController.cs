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

        public IActionResult Index() {
           /* var getDonorRole = context.Roles.Where(x => x.Name == "User").FirstOrDefault();
            ViewBag.Donors = context.UserRoles.Where(x => x.RoleId == getDonorRole.Id).Count();

            var getSuperAdminRole = context.Roles.Where(x => x.Name == "SuperAdmin").FirstOrDefault();
            ViewBag.SuperAdmins = context.UserRoles.Where(x => x.RoleId == getSuperAdminRole.Id).Count();*/

            return View();
        }

        public IActionResult ManageCities()
        {
            return View();
        }
    }
}

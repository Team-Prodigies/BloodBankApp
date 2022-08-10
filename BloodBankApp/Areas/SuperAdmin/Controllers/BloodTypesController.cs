using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    public class BloodTypesController : Controller {

        private readonly ApplicationDbContext context;

        public BloodTypesController(ApplicationDbContext context) {
            this.context = context;
        }

        public IActionResult BloodTypes() {

            var bloodTypes = context.BloodTypes.ToList();
            return View(bloodTypes);
        }
    }
}

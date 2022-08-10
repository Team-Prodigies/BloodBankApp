using BloodBankApp.Data;
using BloodBankApp.Models;
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

        [HttpPost]
        public IActionResult AddNewBloodType(string BloodTypeName) 
        {
            var bloodTypeExists = context.BloodTypes.Where(b => b.BloodTypeName.ToUpper() == BloodTypeName.ToUpper()).FirstOrDefault();
            if (bloodTypeExists == null) 
            {
                BloodType newBloodType = new BloodType();
                newBloodType.BloodTypeName = BloodTypeName;

                context.Add(newBloodType);
                context.SaveChanges();
            }
            return RedirectToAction("BloodTypes");
        }

        [HttpPost]
        public IActionResult EditBloodType(Guid BloodTypeID, string BloodTypeName)
        {
            var bloodTypeExists = context.BloodTypes.Where(b => b.BloodTypeName.ToUpper() == BloodTypeName.ToUpper()).FirstOrDefault();
            if (bloodTypeExists == null)
            {
                var BloodType = context.BloodTypes.Find(BloodTypeID);
                if(BloodType != null)
                {
                    BloodType.BloodTypeName = BloodTypeName;
                    context.Update(BloodType);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("BloodTypes");
        }

        public IActionResult BloodTypes() {

            var bloodTypes = context.BloodTypes.ToList();
            return View(bloodTypes);
        }
    }
}

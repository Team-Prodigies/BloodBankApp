using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class BloodTypesController : Controller {

        private readonly ApplicationDbContext context;

        public BloodTypesController(ApplicationDbContext context) {
            this.context = context;
        }

        public IActionResult CreateBloodType() {

            return View();
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
        public IActionResult EditBloodType(BloodType editBloodType)
        {
            var bloodTypeExists = context.BloodTypes.Where(b => b.BloodTypeName.ToUpper() == editBloodType.BloodTypeName.ToUpper()).FirstOrDefault();
            if (bloodTypeExists == null)
            {
                var BloodType = context.BloodTypes.Find(editBloodType.BloodTypeId);
                if(BloodType != null)
                {
                    BloodType.BloodTypeName = editBloodType.BloodTypeName;
                    context.Update(BloodType);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("BloodTypes");
        }

        [HttpGet]
        public IActionResult EditBloodType(Guid BloodTypeID) {
            var editBloodType = context.BloodTypes.Find(BloodTypeID);

            if(editBloodType == null) {
                return RedirectToAction("BloodTypes");
            }

            return View(editBloodType);
        }

        public IActionResult BloodTypes() {

            var bloodTypes = context.BloodTypes.ToList();
            return View(bloodTypes);
        }
    }
}

using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class HospitalController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly SelectList cityList;
        public HospitalController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
            cityList = new SelectList(context.Cities.ToList(), "CityId", "CityName");
        }

        public IActionResult CreateHospital()
        {
            ViewData["CityId"] = cityList;
            return View();
        }

        public IActionResult ManageHospitals()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHospital(HospitalModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = cityList;
                return View();
            }
            using (var transaction = context.Database.BeginTransaction())
            {
                var location = mapper.Map<Location>(model);
                await context.AddAsync(location);
                try
                {
                    await context.SaveChangesAsync();

                    var hospital = mapper.Map<Hospital>(model);
                    hospital.LocationId = location.LocationId;
                    await context.AddAsync(hospital);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    ViewData["CityId"] = cityList;
                    return View();
                }
            }
            return RedirectToAction(nameof(ManageHospitals));
        }
    }
}

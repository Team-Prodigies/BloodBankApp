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

        public IActionResult ManageHospitals(int pageNumber = 1)
        {
            var skipRows = (pageNumber - 1) * 10;
            var hospitals = context.Hospitals.Include(c => c.City).Skip(skipRows).Take(10).ToList();

            ViewBag.pageNumber = pageNumber;

            return View(hospitals);
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


        [HttpGet]
        public async Task<IActionResult> EditHospital(Guid hospitalId) {

            var hospital = await context.Hospitals.Include(l => l.Location).Include(c => c.City).Where(h => h.HospitalId == hospitalId).FirstOrDefaultAsync();

            if(hospital == null)
            {
                return RedirectToAction(nameof(ManageHospitals));
            }

            ViewData["CityId"] = cityList;
            var editHospital = mapper.Map<HospitalModel>(hospital);
            return View(editHospital);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospital(HospitalModel hospital) {

            if(!ModelState.IsValid)
            {
                return View(hospital.HospitalId);
            }

            var editHospital = mapper.Map<Hospital>(hospital);

            context.Update(editHospital.Location);
            context.Update(editHospital);

            await context.SaveChangesAsync();    
            return RedirectToAction(nameof(EditHospital), new { hospital.HospitalId });
        }

        [HttpGet]
        public async Task<IActionResult> HospitalSearchResults(string searchTerm, int pageNumber = 1) {
            if (searchTerm == null || searchTerm.Trim() == "") {
                return RedirectToAction(nameof(ManageHospitals));
            }
            var skipRows = (pageNumber - 1) * 10;
            var hospitals = await context.Hospitals.Where(hospital => hospital.HospitalName.ToUpper().Contains(searchTerm.ToUpper())).Skip(skipRows).Take(10).ToListAsync();
            

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(hospitals);
        }


    }
}

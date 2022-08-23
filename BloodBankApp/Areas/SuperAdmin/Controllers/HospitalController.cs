using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services;
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
        private readonly IHospitalService hospitalService;
        public HospitalController(ApplicationDbContext context, IMapper mapper, IHospitalService hospitalService)
        {
            this.context = context;
            this.mapper = mapper;
            cityList = new SelectList(context.Cities.ToList(), "CityId", "CityName");
            this.hospitalService = hospitalService;
        }

        public IActionResult CreateHospital()
        {
            ViewData["CityId"] = cityList;
            return View();
        }

        public async Task<IActionResult> ManageHospitals(int pageNumber = 1)
        {
            var hospitals = await hospitalService.GetHospitals(pageNumber);
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
                try
                {
                    await hospitalService.CreateHospital(model);
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
        public async Task<IActionResult> EditHospital(Guid hospitalId) 
        {
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
        public async Task<IActionResult> EditHospital(HospitalModel hospital) 
        {
            if(!ModelState.IsValid)
            {
                return View(hospital);
            }
            await hospitalService.EditHospital(hospital);

            return RedirectToAction(nameof(EditHospital), new { hospital.HospitalId });
        }

        [HttpGet]
        public async Task<IActionResult> HospitalSearchResults(string searchTerm, int pageNumber = 1) 
        {
            if (searchTerm == null || searchTerm.Trim() == "") {
                return RedirectToAction(nameof(ManageHospitals));
            }
            var hospitals = await hospitalService.HospitalSearchResults(searchTerm, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(hospitals);
        }
    }
}

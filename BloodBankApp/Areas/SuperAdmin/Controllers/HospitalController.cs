﻿using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
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
        private readonly ICitiesService _citiesService;
        private readonly IMapper _mapper;
        private readonly SelectList _cityList;
        private readonly IHospitalService _hospitalService;
        public HospitalController(ICitiesService citiesService,
            IMapper mapper,
            IHospitalService hospitalService)
        {
            _citiesService = citiesService;
            _mapper = mapper;
            _cityList = new SelectList(_citiesService.GetCities().Result, "CityId", "CityName");
            _hospitalService = hospitalService;
        }

        public IActionResult CreateHospital()
        {
            ViewData["CityId"] = _cityList;
            return View();
        }

        public async Task<IActionResult> ManageHospitals(int pageNumber = 1)
        {
            var hospitals = await _hospitalService.GetHospitals(pageNumber);
            ViewBag.pageNumber = pageNumber;

            return View(hospitals);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHospital(HospitalModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CityId"] = _cityList;
                return View();
            }
            var result = await _hospitalService.CreateHospital(model);
            if (!result)
            {
                ViewData["CityId"] = _cityList;
                return View();
            }
            return RedirectToAction(nameof(ManageHospitals));
        }


        [HttpGet]
        public async Task<IActionResult> EditHospital(Guid hospitalId) 
        {
            var hospital = await _hospitalService.GetHospital(hospitalId);

            if(hospital == null)
            {
                return RedirectToAction(nameof(ManageHospitals));
            }

            ViewData["CityId"] = _cityList;
            var editHospital = _mapper.Map<HospitalModel>(hospital);
            return View(editHospital);
        }

        [HttpPost]
        public async Task<IActionResult> EditHospital(HospitalModel hospital) 
        {
            if(!ModelState.IsValid)
            {
                return View(hospital);
            }
            await _hospitalService.EditHospital(hospital);

            return RedirectToAction(nameof(EditHospital), new { hospital.HospitalId });
        }

        [HttpGet]
        public async Task<IActionResult> HospitalSearchResults(string searchTerm, int pageNumber = 1) 
        {
            if (searchTerm == null || searchTerm.Trim() == "") {
                return RedirectToAction(nameof(ManageHospitals));
            }
            var hospitals = await _hospitalService.HospitalSearchResults(searchTerm, pageNumber);
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(hospitals);
        }
    }
}

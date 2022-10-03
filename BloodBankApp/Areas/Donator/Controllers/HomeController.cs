using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize(Roles = "Donor")]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IBloodTypesService _bloodTypesService;
        private readonly ICitiesService _citiesService;
        private readonly SelectList _cityList;
        public HomeController(IPostService postService, IBloodTypesService bloodTypesService,ICitiesService citiesService)
        {
            _postService = postService;
            _bloodTypesService = bloodTypesService;
            _citiesService = citiesService;
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
        }

        [Authorize(Policy = Permissions.Donors.ViewDashboard)]
        public async Task<IActionResult> Index(string filterBy = "Normal", int pageNumber = 1)
        {
            var result = await _postService.GetPostsByBloodType(filterBy, pageNumber);

            ViewBag.PageNumber = pageNumber;
            ViewBag.FilterBy = filterBy;
            ViewBag.CityId = _cityList;

            return View(result);
        }

        public async Task<IActionResult> DonationPostSearchResults(string searchTerm, int pageNumber = 1)
        {
            if (searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Index));
            }
            var posts = await _postService.GetPostsBySearch(searchTerm, pageNumber);

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(posts);
        }

        public async Task<IActionResult> DonationPostCityResults(Guid id, int pageNumber = 1)
        {
            
            var posts = await _postService.GetPostsByCity(id, pageNumber);
            var cityName = await _citiesService.GetCity(id);

            ViewBag.PageNumber = pageNumber;
            ViewBag.CityId = _cityList;
            ViewBag.id = id;
            ViewBag.CityName = cityName.CityName;

            return View(posts);
        }

    }
}

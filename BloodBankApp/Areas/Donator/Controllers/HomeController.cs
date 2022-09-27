using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize(Roles = "Donor")]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IBloodTypesService _bloodTypesService;
        public HomeController(IPostService postService, IBloodTypesService bloodTypesService)
        {
            _postService = postService;
            _bloodTypesService = bloodTypesService;
        }

        [Authorize(Policy = Permissions.Donors.ViewDashboard)]
        public async Task<IActionResult> Index(string filterBy = "Normal")
        {
            var result = await _postService.GetPostsByBloodType(filterBy);
            ViewBag.FilterBy = filterBy;
            return View(result);
        }

        public async Task<IActionResult> DonationPostSearchResults(string searchTerm)
        {
            if (searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Index));
            }
            var posts = await _postService.GetPostsBySearch(searchTerm);

            ViewBag.SearchTerm = searchTerm;

            return View(posts);
        }

    }
}

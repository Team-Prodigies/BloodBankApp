using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.Donator.ViewModels;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers {
    [Area("Donator")]
    [Authorize(Roles = "Donor")]
    public class HomeController : Controller {
        private readonly IPostService _postService;
        private readonly ICitiesService _citiesService;
        private readonly SelectList _cityList;
        private readonly INotyfService _notyfService;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public HomeController(IPostService postService,
            IBloodTypesService bloodTypesService,
            ICitiesService citiesService,
            INotyfService notyfService,
            ApplicationDbContext context,
            UserManager<User> userManager) {
            _postService = postService;
            _citiesService = citiesService;
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
            _notyfService = notyfService;
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Policy = Permissions.Donors.ViewDashboard)]
        public async Task<IActionResult> Index(string filterBy = "Normal", int pageNumber = 1) {
            var result = await _postService.GetPostsByBloodType(filterBy, pageNumber);
            var questionnaireQuestions = await _postService.GetQuestionnaireQuestions();

            ViewBag.PageNumber = pageNumber;
            ViewBag.FilterBy = filterBy;
            ViewBag.CityId = _cityList;
            ViewBag.QuestionnaireQuestions = questionnaireQuestions;

            return View(result);
        }

        public async Task<IActionResult> DonationPostSearchResults(string searchTerm, int pageNumber = 1) {
            if (searchTerm == null || searchTerm.Trim() == "") {
                return RedirectToAction(nameof(Index));
            }
            var posts = await _postService.GetPostsBySearch(searchTerm, pageNumber);

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;

            return View(posts);
        }

        public async Task<IActionResult> DonationPostCityResults(Guid id, int pageNumber = 1) {

            var posts = await _postService.GetPostsByCity(id, pageNumber);
            var cityName = await _citiesService.GetCity(id);

            ViewBag.PageNumber = pageNumber;
            ViewBag.CityId = _cityList;
            ViewBag.id = id;
            ViewBag.CityName = cityName.CityName;

            return View(posts);
        }

        [HttpGet]
        public async Task<IActionResult> QuestionnaireAnswers(Guid postId) 
        {
            var getQuestions = await _postService.GetQuestionnaireQuestions();
            var getPost = await _postService.GetPost(postId);

            ViewBag.Post = getPost.NotificationId;
            
            return View(getQuestions);
        }

        [HttpPost]
        public async Task<IActionResult> CheckQuestion(QuestionnaireAnswers answers, Guid postId) 
        {
            var getQuestions = await _postService.GetAllQuestions();
            var getPost = await _postService.GetPost(postId);

            for (int i = 0; i < getQuestions.Count; i++) 
            {
                if (getQuestions[i].Answer != answers.Questions[i].Answer) 
                {
                    _notyfService.Error("Sorry you are not in condition to donate now");
                    return RedirectToAction(nameof(QuestionnaireAnswers), new {postId = getPost.NotificationId });
                }
            }

            var getUser = _userManager.GetUserAsync(User);
            var getDonor = await _context.Donors.FindAsync(getUser.Result.Id);
            
            var getRequests = await _context.DonationRequest.ToListAsync();
            foreach(var req in getRequests) 
                {
                if(req.DonationPostId == getPost.NotificationId) 
                {
                    _notyfService.Error("Sorry but you already made a request");
                    return RedirectToAction(nameof(QuestionnaireAnswers), new {postId = getPost.NotificationId });
                }
            }

            var request = new DonationRequests 
            {
                DonationPostId = getPost.NotificationId,
                DonorId = getDonor.DonorId,
                RequestDate = DateTime.Now
            };

            await _context.DonationRequest.AddAsync(request);
            _context.SaveChanges();

            _notyfService.Success("Donation request has been recorded!");
            return RedirectToAction(nameof(QuestionnaireAnswers), new {postId = getPost.NotificationId});
        }
    }
}

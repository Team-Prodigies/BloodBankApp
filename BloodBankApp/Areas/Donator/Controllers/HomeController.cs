using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.Donator.ViewModels;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.Donator.Controllers
{
    [Area("Donator")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICitiesService _citiesService;
        private readonly SelectList _cityList;
        private readonly INotyfService _notyfService;
        private readonly UserManager<User> _userManager;
        private readonly IDonatorService _donatorService;
        private readonly INotificationService _notificationService;
        private readonly IDonationsService _donationsService;

        public HomeController(IPostService postService,
            ICitiesService citiesService,
            INotyfService notyfService,
            UserManager<User> userManager,
            IDonatorService donatorService,
            INotificationService notificationService,
            IDonationsService donationsService)
        {
            _postService = postService;
            _citiesService = citiesService;
            _cityList = new SelectList(citiesService.GetCities().Result, "CityId", "CityName");
            _notyfService = notyfService;
            _userManager = userManager;
            _donatorService = donatorService;
            _notificationService = notificationService;
            _donationsService = donationsService;
        }

        [Authorize(Policy = Permissions.Donors.ViewDonationPosts)]
        public async Task<IActionResult> Index(string filterBy = "Normal", int pageNumber = 1)
        {
            var result = await _postService.GetPostsByBloodType(filterBy, pageNumber);
            var questionnaireQuestions = await _postService.GetQuestionnaireQuestions();

            ViewBag.PageNumber = pageNumber;
            ViewBag.FilterBy = filterBy;
            ViewBag.CityId = _cityList;
            ViewBag.QuestionnaireQuestions = questionnaireQuestions;

            return View(result);
        }

        [Authorize(Policy = Permissions.Donors.ViewDonationPosts)]
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

        [Authorize(Policy = Permissions.Donors.ViewDonationPosts)]
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

        [Authorize(Policy = Permissions.Donors.ViewQuestionnaire)]
        [HttpGet]
        public async Task<IActionResult> QuestionnaireAnswers(Guid postId)
        {
            var getUser = await _userManager.GetUserAsync(User);
            var getDonationRequest = _postService.GetDonationRequest(postId, getUser.Id);
            if (getDonationRequest) {
                _notyfService.Error("Cant Donate in the same post");
                return RedirectToAction("Index");
            }
            var getQuestions = await _postService.GetQuestionnaireQuestions();
            var getPost = await _postService.GetPost(postId);

            ViewBag.Post = getPost.DonationPostId;

            return View(getQuestions);
        }

        [Authorize(Policy = Permissions.Donors.FillQuestionnaire)]
        [HttpPost]
        public async Task<IActionResult> CheckQuestion(QuestionnaireAnswers answers, Guid postId)
        {
            var getQuestions = await _postService.GetAllQuestions();
            var getPost = await _postService.GetPost(postId);
            var getUser = _userManager.GetUserAsync(User);

            for (var i = 0; i < getQuestions.Count; i++)
            {
                if (getQuestions[i].Answer == answers.Questions[i].Answer) continue;
                _notyfService.Error("Sorry you are not in a good health condition to donate");
                return RedirectToAction(nameof(Index), new { postId = getPost.DonationPostId });
            }

            var result = await _donationsService.AddDonationRequest(getPost.DonationPostId, getUser.Result.Id);
            if(!result) _notyfService.Error("There was a problem sending your request");

            _notyfService.Success("Donation request has been recorded!");
            return RedirectToAction(nameof(Index), new { postId = getPost.DonationPostId });
        }

        [Authorize(Policy = Permissions.Donors.ViewDonationHistory)]
        public async Task<IActionResult> DonationsHistory()
        {
            var donationsHistory = await _donatorService.GetBloodDonationsHistory();
            return View(donationsHistory);
        }

        [Authorize(Policy = Permissions.Donors.ViewNotifications)]
        public async Task<IActionResult> Notifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = await _notificationService.GetNotificationsForDonor(userId);
            return View(notifications);
        }
    }
}
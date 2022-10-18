using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Permission;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers
{
    [Area("HospitalAdmin")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IHospitalService _hospitalService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly SelectList _bloodTypeList;
        private readonly INotyfService _notyfService;
        private readonly IPostService _postService;
        private readonly IBloodTypesService _bloodTypesService;
        private readonly SelectList _postStatus;

        public PostsController(IBloodTypesService bloodTypesService,
            INotyfService notyfService,
            IPostService postService,
            IHospitalService hospitalService,
            INotificationService notificationService,
            UserManager<User> userManager)
        {
            _bloodTypesService = bloodTypesService;
            _hospitalService = hospitalService;
            _notificationService = notificationService;
            _userManager = userManager;
            _notyfService = notyfService;
            _postService = postService;
            _bloodTypeList =
                new SelectList(_bloodTypesService.GetAllBloodTypes().Result, "BloodTypeId", "BloodTypeName");
            _postStatus = new SelectList(Enum.GetValues(typeof(PostStatus))
                .Cast<PostStatus>()
                .ToList(), "PostStatus");
        }

        [Authorize(Policy = Permissions.HospitalAdmin.ManagePosts)]
        public async Task<IActionResult> ManagePosts(string filterBy = "Normal")
        {
            var getUser = _userManager.GetUserId(User);
            var getHospital = await _hospitalService.GetHospitalForMedicalStaff(getUser);
            var result = await _postService.GetPost(getHospital, filterBy);
            ViewBag.FilterBy = filterBy;
            return View(result);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.AddPosts)]
        public IActionResult CreatePosts()
        {
            ViewBag.BloodType = _bloodTypeList;
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.AddPosts)]
        public async Task<IActionResult> CreatePosts(DonationPost post)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.BloodType = _bloodTypeList;
                _notyfService.Error("Your form is not correct. Please try again!");
                return View(nameof(CreatePosts));
            }
            var getUser = _userManager.GetUserId(User);
            var result = await _postService.AddPost(post, getUser);
            if (result == false)
            {
                ViewBag.BloodType = _bloodTypeList;
                _notyfService.Error("The Date of the post is not correct!");
                return View(nameof(CreatePosts));
            }

            var sendNotification = await _notificationService.SendNotificationToDonors(post);
            if (sendNotification)
            {
                _notyfService.Success("Post was created successfully and will notify" +
                                                                      " potential donors");
                return RedirectToAction(nameof(CreatePosts));
            }
            else
            {
                _notyfService.Success("Post successfully created");
                return RedirectToAction(nameof(CreatePosts));
            }
        }

        [HttpGet]
        [Authorize(Policy = Permissions.HospitalAdmin.EditPosts)]
        public async Task<IActionResult> EditPost(Guid DonationPostId)
        {
            var changePost = await _postService.EditPost(DonationPostId);
            ViewBag.BloodType = _bloodTypeList;
            ViewData["PostStatus"] = _postStatus;

            return View(changePost);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.HospitalAdmin.EditPosts)]
        public async Task<IActionResult> EditPost(PostModel post)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("You form is not correct. Please try again!");
                ViewBag.BloodType = _bloodTypeList;
                ViewData["PostStatus"] = _postStatus;
                return View(nameof(EditPost));
            }

            var res = await _postService.EditPosts(post);
            if (res == false)
            {
                _notyfService.Error("You form is not correct. Please try again!");
                ViewBag.BloodType = _bloodTypeList;
                ViewData["PostStatus"] = _postStatus;
                return View(nameof(EditPost));
            }

            _notyfService.Success("Post edited!");
            return RedirectToAction(nameof(ManagePosts));
        }

        [Authorize(Policy = Permissions.HospitalAdmin.DeletePosts)]
        public async Task<IActionResult> DeletePost(Guid DonationPostId)
        {
            var deletePost = await _postService.DeletePost(DonationPostId);
            if (deletePost == false)
            {
                _notyfService.Error("Post not found!");
                return View(nameof(ManagePosts));
            }

            _notyfService.Success("Post Deleted!");
            return RedirectToAction("ManagePosts");
        }
    }
}
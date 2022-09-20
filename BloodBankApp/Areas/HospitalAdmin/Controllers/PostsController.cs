using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.HospitalAdmin.Services.Interfaces;
using BloodBankApp.Areas.HospitalAdmin.ViewModels;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Enums;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.HospitalAdmin.Controllers {
    [Area("HospitalAdmin")]
    [Authorize(Roles = "HospitalAdmin")]
    public class PostsController : Controller {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private SelectList BloodTypeList { get; set; }
        private readonly INotyfService _notyfService;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;
        private readonly IBloodTypesService _bloodTypesService;
        private SelectList PostStatus { get; set; }
        public PostsController(IBloodTypesService bloodTypesService, 
            INotyfService notyfService,
            IPostService postService,
            IMapper mapper,
            ApplicationDbContext context,
            UserManager<User> userManager) {
            _bloodTypesService = bloodTypesService;
            _context = context;
            _userManager = userManager;
            _notyfService = notyfService;
            _postService = postService;
            _mapper = mapper;
            BloodTypeList = new SelectList(_bloodTypesService.GetAllBloodTypes().Result, "BloodTypeId", "BloodTypeName");
            PostStatus = new SelectList(Enum.GetValues(typeof(PostStatus))
               .Cast<PostStatus>()
               .ToList(), "PostStatus");
        }

        public async Task<IActionResult> ManagePosts(string filterBy = "Normal") {
            var getUser = _userManager.GetUserAsync(User);
            var getMedicalStaff = _context.MedicalStaffs.FirstOrDefault(x => x.MedicalStaffId == getUser.Result.Id);
            var getHospital = _context.Hospitals.FirstOrDefault(x => x.HospitalId == getMedicalStaff.HospitalId);

            var result = await _postService.GetPost(getHospital, filterBy);
            ViewBag.FilterBy = filterBy;
            return View(result);
        }

        [HttpGet]
        public IActionResult CreatePosts() { 
            var getUser = _userManager.GetUserAsync(User);
            var getMedicalStaff = _context.MedicalStaffs.FirstOrDefault(x => x.MedicalStaffId == getUser.Result.Id);
            var getHospital = _context.Hospitals.FirstOrDefault(x => x.HospitalId == getMedicalStaff.HospitalId);

            ViewBag.BloodType = BloodTypeList;
            ViewBag.Hospital = getHospital.HospitalId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosts(DonationPost post) {
            if (!ModelState.IsValid) {
                _notyfService.Error("You form is not correct. Please try again!");
                return View(nameof(CreatePosts));
            }

            var result = await _postService.AddPost(post);
            if (result == false) {
                _notyfService.Error("The Date of the post is not correct!");
                return View(nameof(CreatePosts));
            }

            _notyfService.Success("Post successfully created");
            return RedirectToAction(nameof(CreatePosts));
        }

        [HttpGet]
        public async Task<IActionResult> EditPost(Guid notificationId) {
            var changePost = await _postService.EditPost(notificationId);
            ViewBag.BloodType = BloodTypeList;
            ViewData["PostStatus"] = PostStatus;

            return View(changePost);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(PostModel post, Guid notificationId) {
            if (!ModelState.IsValid) {
                    _notyfService.Error("You form is not correct. Please try again!");
                    return View(nameof(EditPost));
            }

            var res = await _postService.EditPosts(post, notificationId);
            if (res == false) {
                _notyfService.Error("You form is not correct. Please try again!");
                return View(nameof(EditPost));
            }

            _notyfService.Success("Post edited!");
            return View(nameof(ManagePosts));
        }

        public async Task<IActionResult> DeletePost(Guid notificationId) {
            var deletePost = await _context.DonationPosts.FindAsync(notificationId);
            if (deletePost == null) {
                _notyfService.Error("Post not found!");
                return View(nameof(ManagePosts));
            }

            _context.DonationPosts.Remove(deletePost);
            await _context.SaveChangesAsync();

            _notyfService.Success("Post Deleted!");
            return RedirectToAction("ManagePosts");
        }

    }
}

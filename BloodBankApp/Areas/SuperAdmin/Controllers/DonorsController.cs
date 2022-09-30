using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using AspNetCoreHero.ToastNotification.Abstractions;
using BloodBankApp.Areas.SuperAdmin.Permission;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize]
    public class DonorsController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IUsersService _usersService;
        private readonly INotyfService _notyfService;
        public DonorsController(IRolesService rolesService,
            IUsersService usersService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _usersService = usersService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Donors.View)]
        public async Task<IActionResult> Donors(string roleFilter = null, int pageNumber = 1, string filterBy = "A-Z")
        {
            var users = await _usersService.GetUsers(roleFilter,pageNumber, filterBy);

            ViewBag.FilterBy = filterBy;
            ViewBag.PageNumber = pageNumber;
            ViewBag.RoleFilter = roleFilter;
            ViewBag.Roles = await _rolesService.GetAllRoleNames();

            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Donors.View)]
        public async Task<IActionResult> DonorSearchResults(string searchTerm, string roleFilter = null, int pageNumber = 1)
        {
            if (searchTerm == null || searchTerm.Trim() == "")
            {
                return RedirectToAction(nameof(Donors));
            }
            var users = await _usersService.UserSearchResults(searchTerm, roleFilter, pageNumber);

            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.RoleFilter = roleFilter;
            ViewBag.Roles = await _rolesService.GetAllRoleNames();

            return View(users);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.Donors.Lock)]
        public async Task<IActionResult> LockoutDonor(Guid userId)
        {
            var user = await _usersService.GetUser(userId);
            if (user == null)
            {
                _notyfService.Warning("User was not found!");
                return RedirectToAction(nameof(Donors));
            }
            await _usersService.LockoutUser(user);
            _notyfService.Success("User " + user.UserName+" has been locked out!");
            return RedirectToAction(nameof(Donors));
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Donors.Unlock)]
        public async Task<IActionResult> UnlockDonor(Guid userId)
        {
            var user = await _usersService.GetUser(userId);
            if (user == null)
            {
                _notyfService.Warning("User was not found!");
                return RedirectToAction(nameof(Donors));
            }
            await _usersService.UnlockUser(user);
            _notyfService.Success("User " + user.UserName + " has been unlocked!");
            return RedirectToAction(nameof(Donors));
        }
    }
}
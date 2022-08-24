using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public async Task<IActionResult> AllRoles()
        {
            var roles = await _rolesService.GetAllRoles();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var check = await _rolesService.CreateRole(model);
            if (!check.Succeeded)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid Id)
        {
            var role = await _rolesService.GetRole(Id);

            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole<Guid> role, Guid Id)
        {
            if (role.Id != Id)
            {
                return NotFound();
            }

            var dbRole = await _rolesService.GetRole(Id);

            if (dbRole == null)
            {
                return NotFound();
            }
            dbRole.Name = role.Name;

            var result = await _rolesService.UpdateRole(dbRole);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(AllRoles));
        }
    }
}

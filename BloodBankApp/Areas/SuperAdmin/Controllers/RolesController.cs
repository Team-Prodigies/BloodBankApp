using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext context, IMapper mapper) {
            this._roleManager = roleManager;
        }

        public async Task<IActionResult> AllRoles() {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel model) {
            if (!ModelState.IsValid) {
                return BadRequest();
            }

            var role = new IdentityRole<Guid>() {
                Name = model.RoleName
            };

            var check = await _roleManager.CreateAsync(role);
            if (!check.Succeeded) {
                return BadRequest();
            }

            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid Id) {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
            if (role == null) {
                return NotFound();
            }

            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole<Guid> role, Guid Id) {
            if (role.Id != Id) {
                return NotFound();
            }
            
            var dbRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            if (dbRole == null) {
                return NotFound();
            }

            dbRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(dbRole);
            if (!result.Succeeded) {
                return NotFound();
            }

            return RedirectToAction("AllRoles");
        }
    }
}

using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers {
    [Area("SuperAdmin")]
    public class RolesController : Controller {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext context, IMapper mapper) {
            this.context = context;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public IActionResult AllRoles() {
            var roles = context.Roles.ToList();
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

            var check = await roleManager.CreateAsync(role);
            if (!check.Succeeded) {
                return BadRequest();
            }

            return RedirectToAction("AllRoles");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid Id) {
            if (Id == null) {
                return NotFound();
            }

            var getRoleId = await roleManager.FindByIdAsync(Id.ToString());
            if (getRoleId == null) {
                return NotFound();
            }

            return View(getRoleId);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole<Guid> role, Guid roleId) {
            if (role.Id == null || roleId == null || role.Id != roleId) {
                return NotFound();
            }
            
            var dbRole = await roleManager.FindByIdAsync(role.Id.ToString());
            if (dbRole == null) {
                return NotFound();
            }

            dbRole.Name = role.Name;
            var result = await roleManager.UpdateAsync(dbRole);
            if (!result.Succeeded) {
                return NotFound();
            }

            return RedirectToAction("AllRoles");
        }
    }
}

using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Areas.SuperAdmin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;
        public RolesController(IRolesService rolesService, IMapper mapper, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _mapper = mapper;
            _notyfService = notyfService;
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
                return View();
            }
            var check = await _rolesService.CreateRole(model);
            if (!check.Succeeded)
            {
                foreach (var error in check.Errors)
                {
                    if (!ModelState.ContainsKey(error.Code))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                return View();
            }
            _notyfService.Success("Role added successfully!");
            return RedirectToAction(nameof(AllRoles));
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _rolesService.GetRole(id);

            if (role == null)
            {
                _notyfService.Warning("Role was not found!");
                return View();
            }
            var roleModel = _mapper.Map<RoleModel>(role);
            return View(roleModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModel role, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (role.Id != id)
            {
                _notyfService.Warning("Role was not found");
                return View();
            }

            var dbRole = await _rolesService.GetRole(id);

            if (dbRole == null)
            {
                _notyfService.Warning("Role was not found");
                return View();
            }
            dbRole.Name = role.RoleName;

            var result = await _rolesService.UpdateRole(dbRole);
           
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (!ModelState.ContainsKey(error.Code))
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
                return View();
            }
            _notyfService.Success("Role was updated!");
            return RedirectToAction(nameof(AllRoles));
        }
    }
}

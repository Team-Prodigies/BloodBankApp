using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BloodBankApp.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> roleManager;
        private readonly ApplicationDbContext context;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext context)
        {
            this.context = context;
            this.roleManager = roleManager;
        }

        public IActionResult AllRoles()
        {
            var roles = context.Roles.ToList();
            return View(roles);
        }
    }
}

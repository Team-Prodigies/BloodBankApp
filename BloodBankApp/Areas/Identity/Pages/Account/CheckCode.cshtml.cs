using System;
using System.Linq;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Data;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    public class CheckCodeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsersService _usersService;

        public CheckCodeModel(ApplicationDbContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        [BindProperty]
        public RegisterModel.RegisterInputModel Input { get; set; }
        public async Task<IActionResult> OnGetAsync(RegisterModel.RegisterInputModel model)
        {
            var donor = await _context.Donors
                .FirstOrDefaultAsync(donor => donor.DonorId == model.Id);

            if (donor == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id, string codeValue)
        {
            var code = await _usersService.CheckDonorsCode(id, codeValue);

            if (code == false)
            {
                return Page();
            }

            return RedirectToAction("Index", "Home", new { area = "Donator" });
        }
    }
}

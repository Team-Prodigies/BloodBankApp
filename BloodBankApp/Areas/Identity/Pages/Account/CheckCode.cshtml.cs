using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;
using BloodBankApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BloodBankApp.Areas.Identity.Pages.Account
{
    public class CheckCodeModel : PageModel
    {
        private readonly IUsersService _usersService;

        public CheckCodeModel(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [BindProperty]
        public RegisterModel.RegisterInputModel Input { get; set; }
        public IActionResult OnGet(RegisterModel.RegisterInputModel model)
        {
            Input = model;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var code = await _usersService.AddNonRegisteredDonor(Input);

            if (!code.Succeeded)
            {
                foreach (var error in code.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }
            return RedirectToAction("Index", "Home", new { area = "Donator" });
        }
    }
}

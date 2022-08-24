using BloodBankApp.Areas.SuperAdmin.Services.Interfaces;
using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services
{
    public class SignInService : ISignInService
    {
        private readonly SignInManager<User> _signInManager;

        public SignInService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync()
        {
            return await _signInManager.GetExternalAuthenticationSchemesAsync();
        }
    }
}

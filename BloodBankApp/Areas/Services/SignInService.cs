using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankApp.Areas.Services.Interfaces;

namespace BloodBankApp.Areas.Services
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

        public async Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(username,password,rememberMe,lockoutOnFailure);
        }

        public async Task RefreshSignInAsync(User user)
        {
            await _signInManager.RefreshSignInAsync(user);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

using BloodBankApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankApp.Areas.SuperAdmin.Services.Interfaces
{
    public interface ISignInService
    {
        Task<IEnumerable<AuthenticationScheme>> GetExternalAuthenticationSchemesAsync();
        Task RefreshSignInAsync(User user);
        Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool lockoutOnFailure);
        Task SignOutAsync();
    }
}

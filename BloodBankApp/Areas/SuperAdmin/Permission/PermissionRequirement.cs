using Microsoft.AspNetCore.Authorization;

namespace BloodBankApp.Areas.SuperAdmin.Permission
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get;}
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}

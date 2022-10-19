using BloodBankApp.Areas.SuperAdmin.ViewModels;
using System.Collections.Generic;
using System.Reflection;
using System;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsViewModel> allPermissions, Type policy)
    {
        var fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (var fi in fields)
        {
            allPermissions.Add(new RoleClaimsViewModel { Value = fi.GetValue(null).ToString(), Type = "Permission" });
        }
    }
}
using System.Collections.Generic;

namespace BloodBankApp.Areas.SuperAdmin.Permission
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }
        public static class Cities
        {
            public const string View = "Permissions.Cities.View";
            public const string Create = "Permissions.Cities.Create";
            public const string Edit = "Permissions.Cities.Edit";
            public const string Delete = "Permissions.Cities.Delete";
        }
        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string ViewPermissions = "Permissions.Roles.Permissions.View";
            public const string EditPermissions = "Permissions.Roles.Permissions.Edit";
            public const string AddPermissions = "Permissions.Roles.Permissions.Add";
        }
        public static class Donors
        {
            public const string View = "Permissions.Donors.View";
            public const string Lock = "Permissions.Donors.Lock";
            public const string Unlock = "Permissions.Donors.Unlock";
            public const string ViewProfile = "Permissions.Donors.Profile.View";
            public const string EditProfile = "Permissions.Donors.Profile.Edit";
            public const string ViewDashboard = "Permissions.Donors.Dashboard.View";
            public const string ChangePassword = "Permissions.Donors.Password.Change";
            public const string DeleteAccount = "Permissions.Donors.Account.Delete";
        }
        public static class SuperAdmin
        {
            public const string Create = "Permissions.SuperAdmin.Create";
            public const string ViewStatistics = "Permissions.SuperAdmin.ViewStatistics";
            public const string ViewProfile = "Permissions.SuperAdmin.Profile.View";
            public const string EditProfile = "Permissions.SuperAdmin.Profile.Edit";
        }
        public static class Hospitals
        {
            public const string View = "Permissions.Hospitals.View";
            public const string Create = "Permissions.Hospitals.Create";
            public const string Edit = "Permissions.Hospitals.Edit";
            public const string Delete = "Permissions.Hospitals.Delete";
        }
        public static class HospitalAdmin
        {
            public const string ViewHospital = "Permissions.Hospital.View";
            public const string EditHospital = "Permissions.Hospital.Edit";
            public const string ViewDashboard = "Permissions.HospitalAdmin.Dashboard.View";
        }
    }
}

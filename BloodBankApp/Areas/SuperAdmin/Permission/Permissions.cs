using System.Collections.Generic;

namespace BloodBankApp.Areas.SuperAdmin.Permission
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Create {module}",
                $"View {module}",
                $"Edit {module}",
                $"Delete {module}"
            };
        }
        public static class Cities
        {
            public const string View = "View Cities";
            public const string Create = "Create Cities";
            public const string Edit = "Edit Cities";
            public const string Delete = "Delete Cities";
        }
        public static class Roles
        {
            public const string View = "View Roles";
            public const string Create = "Create Roles";
            public const string Edit = "Edit Roles";
            public const string Delete = "Delete Roles";
            public const string ViewPermissions = "View Role Permissions";
            public const string EditPermissions = "Edit Role Permissions";
            public const string AddPermissions = "Add Role Permissions";
        }
        public static class Donors
        {
            public const string View = "View Donors";
            public const string Lock = "Lock Donors";
            public const string Unlock = "Unlock Donors";
            public const string ViewProfile = "View Donor Profile";
            public const string EditProfile = "Edit Donor Profile";
            public const string ViewDashboard = "View Donors Dashboard";
            public const string ChangePassword = "Change Donor Password.";
            public const string DeleteAccount = "Delete Donors Account";
        }
        public static class SuperAdmin
        {
            public const string Create = "Create SuperAdmin";
            public const string ViewStatistics = "View SuperAdmin Dashboard";
            public const string ViewProfile = "View SuperAdmin Profile";
            public const string EditProfile = "Edit SuperAdmin Profile";
        }
        public static class Hospitals
        {
            public const string View = "View Hospitals";
            public const string Create = "Create Hospitals";
            public const string Edit = "Edit Hospitals";
            public const string Delete = "Delete Hospitals";
        }
        public static class HospitalAdmin
        {
            public const string ViewHospital = "View Specific Hospital";
            public const string EditHospital = "Edit Specific Hospital";
            public const string ViewDashboard = "View HospitalAdmin Dashboard";
        }
    }
}

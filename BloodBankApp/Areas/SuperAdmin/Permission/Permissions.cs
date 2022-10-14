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
            public const string ViewUserRoles = "View User Roles";
            public const string SetUserRoles = "Set User Roles";
        }
        public static class Donors
        {
            public const string View = "View Donors";
            public const string Lock = "Lock Donors";
            public const string Unlock = "Unlock Donors";
            public const string ViewProfile = "View Donor Profile";
            public const string EditProfile = "Edit Donor Profile";
            public const string ViewDonationPosts = "View Donors Donation Posts";
            public const string ViewDonationHistory = "View Donor Donation History";
            public const string ViewNotifications = "View Donor Notifications";
            public const string ViewQuestionnaire = "View Donor Questionnaire";
            public const string FillQuestionnaire = "Fill Donor Questionnaire";
            public const string ViewHospitalChatRooms = "View Hospital Chat Rooms";
            public const string SendMessageToHospital = "Message Hospital";
            public const string ChangePassword = "Change Donor Password.";
            public const string DeleteAccount = "Delete Donors Account";
        }
        public static class SuperAdmin
        {
            public const string Create = "Create SuperAdmin";
            public const string ViewStatistics = "View SuperAdmin Dashboard";
            public const string ViewProfile = "View SuperAdmin Profile";
            public const string EditProfile = "Edit SuperAdmin Profile";
            public const string ChangePassword = "Change Password";
            public const string CreateQuestionnaire = "Create Questionnaire";
            public const string ManageQuestions = "Manage Questionnaire Questions";
            public const string AddQuestions = "Add Questions To Questionnaire";
            public const string EditQuestions = "Add Questions To Questionnaire";
            public const string DeleteQuestions = "Add Questions To Questionnaire";

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
            public const string ViewProfile = "View HospitalAdmin Profile";
            public const string EditProfile = "Edit HospitalAdmin Profile";
            public const string ViewBloodReserves = "View Hospital Blood Reserve";
            public const string SetBloodReserves = "Set Hospital Blood Reserve";
            public const string ViewBloodDonations = "View Hospital Blood Donations";
            public const string AddBloodDonations = "View Hospital Blood Donations";
            public const string UpdateBloodDonations = "Add Hospital Blood Donation";
            public const string ViewDonationRequests = "Update Hospital Blood Donation Requests";
            public const string ApproveDonationRequests = "Approve Hospital Blood Donation Requests";
            public const string RejectDonationRequests = "Reject Hospital Blood Donation Requests";
            public const string ManageDonors = "Manage Hospital Donors";
            public const string AddDonors = "Add New Donors";
            public const string MessageDonors = "Message Donors";
            public const string FindPotentialDonors = "Find Potential Donors";
            public const string ManagePosts = "Manage Hospital Posts";
            public const string AddPosts = "Add Hospital Posts";
            public const string EditPosts = "Edit Hospital Posts";
            public const string DeletePosts = "Delete Hospital Posts";
            public const string ChangePassword = "Change HospitalAdmin Password";
        }

        public static class Issues
        {
            public const string View = "View Issues";
            public const string Edit = "Edit Hospitals";
            public const string Delete = "Delete Hospitals";
        }
    }
}
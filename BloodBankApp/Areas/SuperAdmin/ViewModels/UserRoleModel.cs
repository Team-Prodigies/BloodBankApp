using System;
using System.Collections.Generic;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public List<SelectedRoleModel> Roles { get; set; }
    }
}

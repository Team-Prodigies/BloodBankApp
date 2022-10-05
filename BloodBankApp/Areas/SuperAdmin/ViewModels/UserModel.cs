using System;

namespace BloodBankApp.Areas.SuperAdmin.ViewModels
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public bool Locked { get; set; }
    }
}
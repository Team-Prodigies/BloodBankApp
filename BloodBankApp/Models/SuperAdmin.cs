using System;
using System.ComponentModel.DataAnnotations;

namespace BloodBankApp.Models
{
    public class SuperAdmin
    {
        [Key]
        public Guid SuperAdminId { get; set; }

        public virtual User User { get; set; }
        public string EmailContact { get; set; }

    }
}
